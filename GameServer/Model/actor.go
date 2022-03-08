package Model

import "github.com/jinzhu/gorm"

type Actor struct {
	gorm.Model
	// Name        string `gorm":type:varchar(20);not null"`
	Roleid      int    `gorm:int(11);not null`
	Username    string `gorm:varchar(11);not null`
	Hair        string `gorm":type:varchar(20);not null"`
	Face        string `gorm":type:varchar(20);not null"`
	Head        string `gorm":type:varchar(20);not null"`
	Cloth       string `gorm":type:varchar(20);not null"`
	Pants       string `gorm":type:varchar(20);not null"`
	Armor       string `gorm":type:varchar(20);not null"`
	Back        string `gorm":type:varchar(20);not null"`
	RightWeapon string `gorm":type:varchar(20);not null"`
	LeftWeapon  string `gorm":type:varchar(20);not null"`
	Body        string `gorm":type:varchar(20);not null"`
	Posx        int    `gorm":type:int(11);not null"`
	Posy        int    `gorm":type:int(11);not null"`
	Posz        int    `gorm":type:int(11);not null"`
	Dir         int    `gorm":type:int(11);not null"`
}
