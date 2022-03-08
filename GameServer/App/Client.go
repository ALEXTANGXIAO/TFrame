package App

import (
	"net"

	common "Tin/Common"
	GameProto "Tin/Gameproto"
	model "Tin/Model"

	"github.com/gogo/protobuf/proto"
	"github.com/wonderivan/logger"
)

type Actor struct {
	RoleId         uint32
	RoomInfo       *Room
	PosPack        *GameProto.PosPack
	AppearancePack *GameProto.AppearancePack
}

type Client struct {
	Addr     string
	Conn     net.Conn
	UDPConn  *net.UDPConn
	UDPAddr  *net.UDPAddr
	Username string
	RoomInfo *Room
	PosPack  *GameProto.PosPack
	Uniid    uint32
	RoleId   uint32
	Actor    *Actor
}

func InstanceClient(conn net.Conn, uniid uint32) *Client {

	rAddr := conn.RemoteAddr()

	client := Client{Addr: rAddr.String(), Conn: conn, Uniid: uniid}

	// url := "http://whois.pconline.com.cn/ipJson.jsp?ip=" + client.Addr + "&json=true"
	// logger.Debug(url)
	// jsonStr := common.Post(url, nil, "application/json;charset=utf-8")
	// logger.Debug(jsonStr)

	return &client
}

func (client *Client) Destroy() {
	client.SavePosition()
}

var ClientBufferHead []byte = []byte{0, 0, 0, 0}

func (client *Client) SendTCP(mainpack *GameProto.MainPack) {
	if client == nil {
		return
	}
	if client.Conn == nil {
		return
	}

	GetDataCenter().sendBuffer(client, mainpack)
}

func (client *Client) SendUDP(mainpack *GameProto.MainPack) {
	if client == nil {
		return
	}
	if client.UDPConn == nil {
		return
	}
	data, err := proto.Marshal(mainpack)
	if err != nil {
		logger.Emer("marshal error: ", err.Error())
		return
	}
	_, err2 := client.UDPConn.WriteToUDP(data, client.UDPAddr)

	if err2 != nil {
		logger.Emer("UDPConn error: ", err2.Error())
	}
}

func (client *Client) UpPos(mainpack *GameProto.MainPack) {
	if client == nil {
		logger.Emer("Error client is nil")
		return
	}
	client.PosPack = mainpack.Playerpack[0].PosPack
	if client.Actor != nil {
		client.Actor.PosPack = client.PosPack
	}
}

func (client *Client) InstanceActor() *Actor {
	DB := common.GetDB()
	var dbActor model.Actor
	DB.Where("roleid = ?", client.RoleId).Find(&dbActor)
	if dbActor.Roleid == 0 {
		dbActor := &model.Actor{Roleid: int(client.RoleId), Username: client.Username}
		DB.Create(dbActor)
	}

	actor := Actor{RoleId: client.RoleId}
	client.Actor = &actor

	return &actor
}

func (client *Client) SavePosition() {
	if client == nil {
		logger.Emer("marshal error: client is nil")
		return
	}

	DB := common.GetDB()

	var dbActor model.Actor

	DB.Where("roleid = ?", client.RoleId).Find(&dbActor)

	actor := client.Actor

	if actor != nil {
		if client.PosPack == nil {
			return
		}
		DB.Model(&dbActor).Update(
			model.Actor{
				Posx: int(actor.PosPack.PosX),
				Posy: int(actor.PosPack.PosY),
				Posz: int(actor.PosPack.PosZ),
			})
	} else {
		logger.Error("Couldn't find actor'", client.RoleId)
	}
}
