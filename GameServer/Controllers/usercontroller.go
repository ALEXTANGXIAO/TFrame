package Controllers

import (
	server "Tin/App"
	common "Tin/Common"
	GameProto "Tin/Gameproto"
	model "Tin/Model"
	"errors"

	"github.com/jinzhu/gorm"
	"golang.org/x/crypto/bcrypt"
)

func InitUserController() {
	controller := server.InstanceController("User", GameProto.RequestCode_User)
	controller.Funcs = map[string]interface{}{}
	controller.Funcs, _ = controller.AddFunction("Login", Login)
	controller.Funcs, _ = controller.AddFunction("Register", Register)
	server.RegisterController(GameProto.RequestCode_User, controller)
}

func Login(client *server.Client, mainpack *GameProto.MainPack, isUdp bool) (*GameProto.MainPack, error) {
	if client == nil {
		return nil, errors.New("client is nil")
	}
	mainpack.Returncode = GameProto.ReturnCode_Success
	client.Username = mainpack.LoginPack.Username
	mainpack.Str = mainpack.LoginPack.Username
	return mainpack, nil
	// if CheckLogin(mainpack, client) {
	// 	mainpack.Returncode = GameProto.ReturnCode_Success
	// 	client.Username = mainpack.LoginPack.Username
	// 	if (client.Actor == nil) || (client.Actor.AppearancePack.Hair == "") {
	// 		mainpack.Str = "noCreate"
	// 	}
	// } else {
	// 	mainpack.Returncode = GameProto.ReturnCode_Fail
	// }
	// return mainpack, nil
}

func Register(client *server.Client, mainpack *GameProto.MainPack, isUdp bool) (*GameProto.MainPack, error) {
	if client == nil {
		return nil, errors.New("client is nil")
	}
	userNameStr := mainpack.LoginPack.Username
	DB := common.GetDB()
	if isUserExist(DB, userNameStr) {
		mainpack.Str = "已经存在用户"
		mainpack.Returncode = GameProto.ReturnCode_Fail
		return mainpack, nil
	} else {
		//创建用户
		//加密密码
		roleid := InstanceRoleId(DB)
		hasedPassword, _ := bcrypt.GenerateFromPassword([]byte(mainpack.LoginPack.Password), bcrypt.DefaultCost)
		newUser := model.User{
			Name:     userNameStr,
			Username: userNameStr,
			Password: string(hasedPassword),
			Roleid:   roleid,
		}
		DB.Create(&newUser)
	}
	mainpack.Returncode = GameProto.ReturnCode_Success
	return mainpack, nil
}

func CheckLogin(mainpack *GameProto.MainPack, client *server.Client) bool {
	//todo check mysqlLogin
	DB := common.GetDB()
	//判断账号号是否存在
	var user model.User
	DB.Where("username = ?", mainpack.LoginPack.Username).First(&user)
	if user.ID == 0 {
		mainpack.Str = "用户不存在"
		return false
	}

	//判断账号密码正确与否
	if err := bcrypt.CompareHashAndPassword([]byte(user.Password), []byte(mainpack.LoginPack.Password)); err != nil {
		mainpack.Str = "密码错误"
		return false
	}

	client.RoleId = uint32(user.Roleid)
	client.InstanceActor()
	return true
}

func isUserExist(db *gorm.DB, userName string) bool {
	var user model.User
	db.Where("username = ?", userName).First(&user)
	return user.ID != 0
}

func InstanceRoleId(db *gorm.DB) int {
	// 获取最后一条记录，按主键排序
	var user model.User
	db.Last(&user)
	Roleid := user.ID + 1000
	return int(Roleid)
}
