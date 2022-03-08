package Controllers

import (
	server "Tin/App"
	common "Tin/Common"
	GameProto "Tin/Gameproto"
	model "Tin/Model"
	"errors"

	"github.com/wonderivan/logger"
)

func InitGameController() {
	controller := server.InstanceController("Game", GameProto.RequestCode_Game)
	controller.Funcs = map[string]interface{}{}
	controller.Funcs, _ = controller.AddFunction("UpPos", UpPos)
	controller.Funcs, _ = controller.AddFunction("UpdateState", UpdateState)
	server.RegisterController(GameProto.RequestCode_Game, controller)
}

func UpPos(client *server.Client, mainpack *GameProto.MainPack, isUdp bool) (*GameProto.MainPack, error) {
	if client == nil {
		return nil, errors.New("client is nil")
	}
	if client.RoomInfo == nil {
		return nil, errors.New("client roomInfo is nil")
	}
	client.RoomInfo.BroadcastUDP(client, mainpack)
	client.UpPos(mainpack)
	return nil, nil
}

func UpdateState(client *server.Client, mainpack *GameProto.MainPack, isUdp bool) (*GameProto.MainPack, error) {
	if client == nil {
		return nil, errors.New("client is nil")
	}
	if client.Actor == nil {
		return nil, errors.New("actor is nil")
	}
	if mainpack == nil {
		return nil, errors.New("mainpack is nil")
	} else {
		if mainpack.Playerpack == nil || len(mainpack.Playerpack) <= 0 {
			return nil, errors.New("mainpack.Playerpack is nil")
		}
	}

	appearance := mainpack.Playerpack[0].Appearance

	logger.Info(appearance)

	DB := common.GetDB()
	var dbActor model.Actor
	DB.Where("roleid = ?", client.RoleId).Find(&dbActor)

	DB.Model(&dbActor).Update(
		model.Actor{
			Hair:        appearance.Hair,
			Face:        appearance.Face,
			Head:        appearance.Head,
			Cloth:       appearance.Cloth,
			Pants:       appearance.Pants,
			Armor:       appearance.Armor,
			Back:        appearance.Back,
			RightWeapon: appearance.RightWeapon,
			LeftWeapon:  appearance.LeftWeapon,
			Body:        appearance.Body,
		})

	returnProto, err := server.GetDataCenter().BuildProto(GameProto.RequestCode_Game, GameProto.ActionCode_UpdateState, GameProto.ReturnCode_Success)
	if err != nil {
		return nil, err
	} else {
		return returnProto, nil
	}
}
