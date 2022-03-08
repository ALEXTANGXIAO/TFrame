package Model

import "github.com/jinzhu/gorm"

type User struct {
	gorm.Model
	Name     string `gorm":type:varchar(20);not null"`
	Username string `gorm:varchar(11);not null`
	Password string `gorm:size:255;not null`
	Roleid   int    `gorm:int(11);not null`
}
