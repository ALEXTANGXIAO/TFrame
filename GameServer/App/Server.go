package App

import (
	GameProto "Tin/Gameproto"
	"Tin/GoPool"
	"errors"
	"net"
	"os"
	"runtime"
	"strings"

	"github.com/gogo/protobuf/proto"
	"github.com/panjf2000/ants"
	"github.com/spf13/viper"
	"github.com/wonderivan/logger"
)

//ConnNet Ants 连接池专用
type ConnNet struct {
	conn  net.Conn
	uniid uint32
}

var ConnMap = make(map[uint32]net.Conn)

var ClientList = []*Client{}

var RoomList = []*Room{}

type ServerImp struct {
	TCPPort     string
	TCPListener *net.TCPListener
	UDPConn     *net.UDPConn
	UDPAddr     *net.UDPAddr
}

type Server interface {
	Start() error
	StartTCPServer() error
	StartUDPServer() error
}

var GoPoolMgr *ants.PoolWithFunc

//开启服务器
func (server *ServerImp) Start() error {
	logger.Info("Starting Game Server")

	CreateRooms()
	go server.StartTCPServer()
	go server.StartUDPServer()
	return nil
}

func (server *ServerImp) StartTCPServer() error {
	logger.Info("Starting TCP Server", server.TCPPort)
	tcpAddr, err := net.ResolveTCPAddr("tcp4", server.TCPPort)
	checkErr(err)
	listener, err := net.ListenTCP("tcp", tcpAddr)
	checkErr(err)
	server.TCPListener = listener

	poolOpen := viper.GetBool("GoPool.Open")
	if poolOpen {
		GoPoolMgr, _ = GoPool.InitPool(viper.GetInt("GoPool.poolSize"), handleClientPool)
		logger.Info("GoPoolMgr:", GoPoolMgr)
	}

	var connid uint32
	for {
		conn, err := server.TCPListener.Accept()
		if err != nil {
			logger.Emer("accept failed, err:", err)
			continue
		}
		connid++
		uniid := connid
		ConnMap[uniid] = conn

		if poolOpen {
			task := &ConnNet{conn: conn, uniid: uniid}
			//使用Go Ants goroutine
			err := GoPoolMgr.Invoke(task)
			if err != nil {
				logger.Emer(" GoPoolMgr.Invok failed, err:", err)
				continue
			}
		} else {
			go handleClient(conn, uniid) //普通goroutine
		}
	}
}

func (server *ServerImp) StartUDPServer() error {
	//Start Udp Server
	UDPPort := viper.GetString("server.UDPPort")
	logger.Info("Starting UDP Server", UDPPort)
	uDPAddr, err := net.ResolveUDPAddr("udp", UDPPort)
	server.UDPAddr = uDPAddr
	checkErr(err)
	uDPConn, err := net.ListenUDP("udp", server.UDPAddr)
	server.UDPConn = uDPConn
	checkErr(err)

	for {
		buf := make([]byte, 1024)
		if server.UDPConn == nil {
			return nil
		}
		n, remoteAddr, err := server.UDPConn.ReadFromUDP(buf)
		if err != nil {
			logger.Info("failed to read UDP msg because of ", err.Error())
			return nil
		}
		errBuffer := handBufferUdp(server.UDPConn, remoteAddr, buf[0:n])
		if errBuffer != nil {
			logger.Emer(server.UDPConn, "UDPConn failed")
			return nil
		}
	}
}

func StartServer(port string) Server {
	var server Server = &ServerImp{TCPPort: port}
	err := server.Start()
	if err != nil {
		logger.Emer(err)
		return nil
	}
	return server
}

func handleClientPool(data interface{}) {
	conn_net := data.(*ConnNet)
	conn := conn_net.conn
	uniid := conn_net.uniid
	defer conn.Close()
	logger.Debug("runtime.NumGoroutine()", runtime.NumGoroutine())
	client := InstanceClient(conn, uniid)
	ClientList = append(ClientList, client)
	buf := make([]byte, 1024)
	for {
		cnt, err := conn.Read(buf)
		if err != nil {
			logger.Emer(conn.RemoteAddr())
			logger.Emer(" conn.Read error", err)
			RemoveClient(client)
			break
		}
		err2 := GetDataCenter().handBuffer(client, buf[BufferHeadLength:cnt])
		if err != nil {
			logger.Emer(conn.RemoteAddr())
			logger.Emer("handBuffer error", err2)
			RemoveClient(client)
			break
		}
	}
}

func handleClient(conn net.Conn, uniid uint32) {
	defer conn.Close()
	logger.Debug("runtime.NumGoroutine()", runtime.NumGoroutine())
	client := InstanceClient(conn, uniid)
	ClientList = append(ClientList, client)
	buf := make([]byte, 1024)
	for {
		cnt, err := conn.Read(buf)
		if err != nil {
			logger.Emer(conn.RemoteAddr())
			logger.Emer(" conn.Read error", err)
			RemoveClient(client)
			break
		}
		err2 := GetDataCenter().handBuffer(client, buf[BufferHeadLength:cnt])
		if err != nil {
			logger.Emer(conn.RemoteAddr())
			logger.Emer("handBuffer error", err2)
			RemoveClient(client)
			break
		}
	}
}

func handBufferUdp(conn *net.UDPConn, remoteAddr *net.UDPAddr, buf []byte) error {
	mainPack := &GameProto.MainPack{}
	protoData := buf
	err := proto.Unmarshal(protoData, mainPack)
	if err != nil {
		return err
	}
	client, err := GetClient(mainPack.User)
	if err != nil {
		return err
	}
	if client.UDPConn == nil {
		client.UDPConn = conn
	}
	if client.UDPAddr == nil {
		client.UDPAddr = remoteAddr
	}
	handleReqUdp(client, mainPack, true)
	return nil
}

func RemoveClient(client *Client) {
	room := client.RoomInfo
	room.RemoveClient(client)

	client.Destroy()

	ClientList = removeClient(ClientList, client)

	_, ok := ConnMap[client.Uniid]
	if ok {
		delete(ConnMap, client.Uniid)
	}
	logger.Debug("Rmv client from Server =>", client.Addr, "Uniid :=>", client.Uniid, "  ClientCount =>", len(ClientList))
	client = nil
}

func removeClient(values []*Client, val *Client) []*Client {
	if len(values) <= 0 {
		return values
	}

	res := []*Client{}

	for i := 0; i < len(values); i++ {
		if values[i] == val {
			continue
		}
		v := values[i]
		res = append(res, v)
	}
	return res
}

func checkErr(err error) {
	if err != nil {
		logger.Emer(os.Stderr, "Fatal error: %s", err.Error())
		// os.Exit(1)
	}
}

func GetClient(clientName string) (*Client, error) {
	for i := 0; i < len(ClientList); i++ {
		if ClientList[i] == nil {
			continue
		}
		if ClientList[i].Username == "" {
			logger.Emer("Had client Username = nil 👉", clientName)
			ClientList[i].Username = clientName
			return ClientList[i], nil
		}
		if strings.Compare(ClientList[i].Username, clientName) == 0 {
			return ClientList[i], nil
		}
	}

	logger.Emer("Had not client 👉", clientName)
	return nil, errors.New("had not client")
}
