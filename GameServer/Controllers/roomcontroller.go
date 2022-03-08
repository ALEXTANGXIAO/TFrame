package Controllers

import (
	server "Tin/App"
	GameProto "Tin/Gameproto"
	"errors"
	"runtime"
	"time"

	"github.com/wonderivan/logger"
)

func InitRoomController() {
	controller := server.InstanceController("Room", GameProto.RequestCode_Room)
	controller.Funcs = map[string]interface{}{}
	controller.Funcs, _ = controller.AddFunction("JoinRoom", JoinRoom)
	controller.Funcs, _ = controller.AddFunction("Chat", Chat)
	controller.Funcs, _ = controller.AddFunction("CreateRoom", CreateRoom)
	server.RegisterController(GameProto.RequestCode_Room, controller)
}

func Chat(client *server.Client, mainpack *GameProto.MainPack, isUdp bool) (*GameProto.MainPack, error) {
	if client == nil {
		return nil, errors.New("client is nill")
	}
	if client.RoomInfo == nil {
		return nil, errors.New("client roomInfo is nill")
	}
	mainpack.User = client.Username

	mainpack.Returncode = GameProto.ReturnCode_Success
	client.RoomInfo.BroadcastTCP(client, mainpack)
	return nil, nil
}

func CreateRoom(client *server.Client, mainpack *GameProto.MainPack, isUdp bool) (*GameProto.MainPack, error) {
	if client == nil {
		return nil, errors.New("client is nill")
	}
	if client.RoomInfo != nil {
		mainpack.Returncode = GameProto.ReturnCode_Fail
		mainpack.Str = "client is in Room"
		return mainpack, errors.New("client is in nill")
	}
	if mainpack.Roompack == nil {
		mainpack.Returncode = GameProto.ReturnCode_Fail
		return mainpack, errors.New("roompack is in nill")
	}

	roomPack := mainpack.Roompack[0]
	roomName := roomPack.Roomname

	for _, v := range server.RoomList {
		if v.RoomPack.Roomname == roomName {
			mainpack.Returncode = GameProto.ReturnCode_Fail
			return mainpack, errors.New("room had Instance")
		}
	}

	room := server.InstanceRoom(roomPack)
	server.RoomList = append(server.RoomList, &room)

	room.Join(client)

	mainpack.Returncode = GameProto.ReturnCode_Success
	return mainpack, nil
}

func JoinRoom(client *server.Client, mainpack *GameProto.MainPack, isUdp bool) (*GameProto.MainPack, error) {
	if client == nil {
		return nil, errors.New("client is nil")
	}

	go func() {
		ticker := time.NewTimer(time.Second * 2)
		<-ticker.C //阻塞，1秒以后继续执行
		ticker.Stop()
		if len(server.RoomList) == 0 {
			logger.Error("RoomList count is empty")
		} else {
			roomName := mainpack.Str
			if roomName == "" {
				room := server.RoomList[0]
				room.Join(client)
			} else {
				for i := 0; i < len(server.RoomList); i++ {
					if server.RoomList[i].RoomPack.Roomname == roomName {
						server.RoomList[i].Join(client)
					}
				}
			}
		}
		runtime.Goexit()
	}()

	client.InstanceActor()

	mainpack.Returncode = GameProto.ReturnCode_Success
	return mainpack, nil
}
