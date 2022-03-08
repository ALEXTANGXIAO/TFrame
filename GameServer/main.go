package main

import (
	"Tin/App"
	"os"

	common "Tin/Common"
	controllers "Tin/Controllers"

	"github.com/spf13/viper"
	"github.com/wonderivan/logger"
)

func main() {
	InitLogger()
	InitConfig()
	InitDatabase()
	InitControllers()
	go App.StartServer(viper.GetString("server.TCPPort"))
	select {}
}

func InitConfig() {
	workDir, _ := os.Getwd()
	viper.SetConfigName("config")
	viper.SetConfigType("yml")
	viper.AddConfigPath(workDir + "/config")

	err := viper.ReadInConfig()
	if err != nil {
		panic(err)
	}
}

func InitDatabase() {
	db := common.InitDB()
	logger.Debug("InitDB: ", db)
}

func InitControllers() {
	controllers.InitUserController()
	controllers.InitRoomController()
	controllers.InitGameController()
}

func InitLogger() {
	print("\n 	 _   _      _ _    __        __         _     _ \n	| | | | ___| | | __\\ \\      / /__  _ __| | __| |\n	| |_| |/ _ \\ | |/ _ \\ \\ /\\ / / _ \\| '__| |/ _` |\n	|  _  |  __/ | | (_) \\ V  V / (_) | |  | | (_| |\n	|_| |_|\\___|_|_|\\___/ \\_/\\_/ \\___/|_|  |_|\\__,_|")
	logger.SetLogger("config/log.json")
}
