// Code generated by protoc-gen-gogo. DO NOT EDIT.
// source: Goproto/TGameProtocol.proto

package GameProtocol

import (
	fmt "fmt"
	proto "github.com/gogo/protobuf/proto"
	math "math"
)

// Reference imports to suppress errors if they are not otherwise used.
var _ = proto.Marshal
var _ = fmt.Errorf
var _ = math.Inf

// This is a compile-time assertion to ensure that this generated file
// is compatible with the proto package it is being compiled against.
// A compilation error at this line likely means your copy of the
// proto package needs to be updated.
const _ = proto.GoGoProtoPackageIsVersion3 // please upgrade the proto package

type RequestCode int32

const (
	RequestCode_RequestNone RequestCode = 0
	RequestCode_User        RequestCode = 1
	RequestCode_Room        RequestCode = 2
	RequestCode_Game        RequestCode = 3
	RequestCode_Heart       RequestCode = 4
)

var RequestCode_name = map[int32]string{
	0: "RequestNone",
	1: "User",
	2: "Room",
	3: "Game",
	4: "Heart",
}

var RequestCode_value = map[string]int32{
	"RequestNone": 0,
	"User":        1,
	"Room":        2,
	"Game":        3,
	"Heart":       4,
}

func (x RequestCode) String() string {
	return proto.EnumName(RequestCode_name, int32(x))
}

func (RequestCode) EnumDescriptor() ([]byte, []int) {
	return fileDescriptor_87d87b5d90861b2d, []int{0}
}

type ActionCode int32

const (
	ActionCode_ActionNone      ActionCode = 0
	ActionCode_Register        ActionCode = 1
	ActionCode_Login           ActionCode = 2
	ActionCode_CreateRoom      ActionCode = 3
	ActionCode_FindRoom        ActionCode = 4
	ActionCode_PlayerList      ActionCode = 5
	ActionCode_JoinRoom        ActionCode = 6
	ActionCode_Exit            ActionCode = 7
	ActionCode_Chat            ActionCode = 8
	ActionCode_StartGame       ActionCode = 9
	ActionCode_Starting        ActionCode = 10
	ActionCode_UpdateState     ActionCode = 11
	ActionCode_ExitGame        ActionCode = 12
	ActionCode_UpCharacterList ActionCode = 13
	ActionCode_UpPos           ActionCode = 14
	ActionCode_Fire            ActionCode = 15
	ActionCode_HeartBeat       ActionCode = 16
	ActionCode_AddCharacter    ActionCode = 17
	ActionCode_RemoveCharacter ActionCode = 18
)

var ActionCode_name = map[int32]string{
	0:  "ActionNone",
	1:  "Register",
	2:  "Login",
	3:  "CreateRoom",
	4:  "FindRoom",
	5:  "PlayerList",
	6:  "JoinRoom",
	7:  "Exit",
	8:  "Chat",
	9:  "StartGame",
	10: "Starting",
	11: "UpdateState",
	12: "ExitGame",
	13: "UpCharacterList",
	14: "UpPos",
	15: "Fire",
	16: "HeartBeat",
	17: "AddCharacter",
	18: "RemoveCharacter",
}

var ActionCode_value = map[string]int32{
	"ActionNone":      0,
	"Register":        1,
	"Login":           2,
	"CreateRoom":      3,
	"FindRoom":        4,
	"PlayerList":      5,
	"JoinRoom":        6,
	"Exit":            7,
	"Chat":            8,
	"StartGame":       9,
	"Starting":        10,
	"UpdateState":     11,
	"ExitGame":        12,
	"UpCharacterList": 13,
	"UpPos":           14,
	"Fire":            15,
	"HeartBeat":       16,
	"AddCharacter":    17,
	"RemoveCharacter": 18,
}

func (x ActionCode) String() string {
	return proto.EnumName(ActionCode_name, int32(x))
}

func (ActionCode) EnumDescriptor() ([]byte, []int) {
	return fileDescriptor_87d87b5d90861b2d, []int{1}
}

type ReturnCode int32

const (
	ReturnCode_ReturnNone ReturnCode = 0
	ReturnCode_Success    ReturnCode = 1
	ReturnCode_Fail       ReturnCode = 2
	ReturnCode_NoneRoom   ReturnCode = 3
)

var ReturnCode_name = map[int32]string{
	0: "ReturnNone",
	1: "Success",
	2: "Fail",
	3: "NoneRoom",
}

var ReturnCode_value = map[string]int32{
	"ReturnNone": 0,
	"Success":    1,
	"Fail":       2,
	"NoneRoom":   3,
}

func (x ReturnCode) String() string {
	return proto.EnumName(ReturnCode_name, int32(x))
}

func (ReturnCode) EnumDescriptor() ([]byte, []int) {
	return fileDescriptor_87d87b5d90861b2d, []int{2}
}

type MainPack struct {
	Requestcode          RequestCode   `protobuf:"varint,1,opt,name=requestcode,proto3,enum=GameProtocol.RequestCode" json:"requestcode,omitempty"`
	Actioncode           ActionCode    `protobuf:"varint,2,opt,name=actioncode,proto3,enum=GameProtocol.ActionCode" json:"actioncode,omitempty"`
	Returncode           ReturnCode    `protobuf:"varint,3,opt,name=returncode,proto3,enum=GameProtocol.ReturnCode" json:"returncode,omitempty"`
	LoginPack            *LoginPack    `protobuf:"bytes,4,opt,name=loginPack,proto3" json:"loginPack,omitempty"`
	Str                  string        `protobuf:"bytes,5,opt,name=str,proto3" json:"str,omitempty"`
	Roompack             []*RoomPack   `protobuf:"bytes,6,rep,name=roompack,proto3" json:"roompack,omitempty"`
	Playerpack           []*PlayerPack `protobuf:"bytes,7,rep,name=playerpack,proto3" json:"playerpack,omitempty"`
	User                 string        `protobuf:"bytes,8,opt,name=user,proto3" json:"user,omitempty"`
	XXX_NoUnkeyedLiteral struct{}      `json:"-"`
	XXX_unrecognized     []byte        `json:"-"`
	XXX_sizecache        int32         `json:"-"`
}

func (m *MainPack) Reset()         { *m = MainPack{} }
func (m *MainPack) String() string { return proto.CompactTextString(m) }
func (*MainPack) ProtoMessage()    {}
func (*MainPack) Descriptor() ([]byte, []int) {
	return fileDescriptor_87d87b5d90861b2d, []int{0}
}
func (m *MainPack) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_MainPack.Unmarshal(m, b)
}
func (m *MainPack) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_MainPack.Marshal(b, m, deterministic)
}
func (m *MainPack) XXX_Merge(src proto.Message) {
	xxx_messageInfo_MainPack.Merge(m, src)
}
func (m *MainPack) XXX_Size() int {
	return xxx_messageInfo_MainPack.Size(m)
}
func (m *MainPack) XXX_DiscardUnknown() {
	xxx_messageInfo_MainPack.DiscardUnknown(m)
}

var xxx_messageInfo_MainPack proto.InternalMessageInfo

func (m *MainPack) GetRequestcode() RequestCode {
	if m != nil {
		return m.Requestcode
	}
	return RequestCode_RequestNone
}

func (m *MainPack) GetActioncode() ActionCode {
	if m != nil {
		return m.Actioncode
	}
	return ActionCode_ActionNone
}

func (m *MainPack) GetReturncode() ReturnCode {
	if m != nil {
		return m.Returncode
	}
	return ReturnCode_ReturnNone
}

func (m *MainPack) GetLoginPack() *LoginPack {
	if m != nil {
		return m.LoginPack
	}
	return nil
}

func (m *MainPack) GetStr() string {
	if m != nil {
		return m.Str
	}
	return ""
}

func (m *MainPack) GetRoompack() []*RoomPack {
	if m != nil {
		return m.Roompack
	}
	return nil
}

func (m *MainPack) GetPlayerpack() []*PlayerPack {
	if m != nil {
		return m.Playerpack
	}
	return nil
}

func (m *MainPack) GetUser() string {
	if m != nil {
		return m.User
	}
	return ""
}

type LoginPack struct {
	Username             string   `protobuf:"bytes,1,opt,name=username,proto3" json:"username,omitempty"`
	Password             string   `protobuf:"bytes,2,opt,name=password,proto3" json:"password,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *LoginPack) Reset()         { *m = LoginPack{} }
func (m *LoginPack) String() string { return proto.CompactTextString(m) }
func (*LoginPack) ProtoMessage()    {}
func (*LoginPack) Descriptor() ([]byte, []int) {
	return fileDescriptor_87d87b5d90861b2d, []int{1}
}
func (m *LoginPack) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_LoginPack.Unmarshal(m, b)
}
func (m *LoginPack) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_LoginPack.Marshal(b, m, deterministic)
}
func (m *LoginPack) XXX_Merge(src proto.Message) {
	xxx_messageInfo_LoginPack.Merge(m, src)
}
func (m *LoginPack) XXX_Size() int {
	return xxx_messageInfo_LoginPack.Size(m)
}
func (m *LoginPack) XXX_DiscardUnknown() {
	xxx_messageInfo_LoginPack.DiscardUnknown(m)
}

var xxx_messageInfo_LoginPack proto.InternalMessageInfo

func (m *LoginPack) GetUsername() string {
	if m != nil {
		return m.Username
	}
	return ""
}

func (m *LoginPack) GetPassword() string {
	if m != nil {
		return m.Password
	}
	return ""
}

type RoomPack struct {
	Roomname             string   `protobuf:"bytes,1,opt,name=roomname,proto3" json:"roomname,omitempty"`
	Maxnum               int32    `protobuf:"varint,2,opt,name=maxnum,proto3" json:"maxnum,omitempty"`
	Curnum               int32    `protobuf:"varint,3,opt,name=curnum,proto3" json:"curnum,omitempty"`
	State                int32    `protobuf:"varint,4,opt,name=state,proto3" json:"state,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *RoomPack) Reset()         { *m = RoomPack{} }
func (m *RoomPack) String() string { return proto.CompactTextString(m) }
func (*RoomPack) ProtoMessage()    {}
func (*RoomPack) Descriptor() ([]byte, []int) {
	return fileDescriptor_87d87b5d90861b2d, []int{2}
}
func (m *RoomPack) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_RoomPack.Unmarshal(m, b)
}
func (m *RoomPack) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_RoomPack.Marshal(b, m, deterministic)
}
func (m *RoomPack) XXX_Merge(src proto.Message) {
	xxx_messageInfo_RoomPack.Merge(m, src)
}
func (m *RoomPack) XXX_Size() int {
	return xxx_messageInfo_RoomPack.Size(m)
}
func (m *RoomPack) XXX_DiscardUnknown() {
	xxx_messageInfo_RoomPack.DiscardUnknown(m)
}

var xxx_messageInfo_RoomPack proto.InternalMessageInfo

func (m *RoomPack) GetRoomname() string {
	if m != nil {
		return m.Roomname
	}
	return ""
}

func (m *RoomPack) GetMaxnum() int32 {
	if m != nil {
		return m.Maxnum
	}
	return 0
}

func (m *RoomPack) GetCurnum() int32 {
	if m != nil {
		return m.Curnum
	}
	return 0
}

func (m *RoomPack) GetState() int32 {
	if m != nil {
		return m.State
	}
	return 0
}

type PlayerPack struct {
	Playername           string          `protobuf:"bytes,1,opt,name=playername,proto3" json:"playername,omitempty"`
	PlayerID             string          `protobuf:"bytes,2,opt,name=playerID,proto3" json:"playerID,omitempty"`
	Hp                   int32           `protobuf:"varint,3,opt,name=hp,proto3" json:"hp,omitempty"`
	PosPack              *PosPack        `protobuf:"bytes,4,opt,name=posPack,proto3" json:"posPack,omitempty"`
	PlayerJob            int32           `protobuf:"varint,5,opt,name=playerJob,proto3" json:"playerJob,omitempty"`
	Appearance           *AppearancePack `protobuf:"bytes,6,opt,name=appearance,proto3" json:"appearance,omitempty"`
	XXX_NoUnkeyedLiteral struct{}        `json:"-"`
	XXX_unrecognized     []byte          `json:"-"`
	XXX_sizecache        int32           `json:"-"`
}

func (m *PlayerPack) Reset()         { *m = PlayerPack{} }
func (m *PlayerPack) String() string { return proto.CompactTextString(m) }
func (*PlayerPack) ProtoMessage()    {}
func (*PlayerPack) Descriptor() ([]byte, []int) {
	return fileDescriptor_87d87b5d90861b2d, []int{3}
}
func (m *PlayerPack) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_PlayerPack.Unmarshal(m, b)
}
func (m *PlayerPack) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_PlayerPack.Marshal(b, m, deterministic)
}
func (m *PlayerPack) XXX_Merge(src proto.Message) {
	xxx_messageInfo_PlayerPack.Merge(m, src)
}
func (m *PlayerPack) XXX_Size() int {
	return xxx_messageInfo_PlayerPack.Size(m)
}
func (m *PlayerPack) XXX_DiscardUnknown() {
	xxx_messageInfo_PlayerPack.DiscardUnknown(m)
}

var xxx_messageInfo_PlayerPack proto.InternalMessageInfo

func (m *PlayerPack) GetPlayername() string {
	if m != nil {
		return m.Playername
	}
	return ""
}

func (m *PlayerPack) GetPlayerID() string {
	if m != nil {
		return m.PlayerID
	}
	return ""
}

func (m *PlayerPack) GetHp() int32 {
	if m != nil {
		return m.Hp
	}
	return 0
}

func (m *PlayerPack) GetPosPack() *PosPack {
	if m != nil {
		return m.PosPack
	}
	return nil
}

func (m *PlayerPack) GetPlayerJob() int32 {
	if m != nil {
		return m.PlayerJob
	}
	return 0
}

func (m *PlayerPack) GetAppearance() *AppearancePack {
	if m != nil {
		return m.Appearance
	}
	return nil
}

type AppearancePack struct {
	Hair                 string   `protobuf:"bytes,1,opt,name=Hair,proto3" json:"Hair,omitempty"`
	Face                 string   `protobuf:"bytes,2,opt,name=Face,proto3" json:"Face,omitempty"`
	Head                 string   `protobuf:"bytes,3,opt,name=Head,proto3" json:"Head,omitempty"`
	Cloth                string   `protobuf:"bytes,4,opt,name=Cloth,proto3" json:"Cloth,omitempty"`
	Pants                string   `protobuf:"bytes,5,opt,name=Pants,proto3" json:"Pants,omitempty"`
	Armor                string   `protobuf:"bytes,6,opt,name=Armor,proto3" json:"Armor,omitempty"`
	Back                 string   `protobuf:"bytes,7,opt,name=Back,proto3" json:"Back,omitempty"`
	RightWeapon          string   `protobuf:"bytes,8,opt,name=RightWeapon,proto3" json:"RightWeapon,omitempty"`
	LeftWeapon           string   `protobuf:"bytes,9,opt,name=LeftWeapon,proto3" json:"LeftWeapon,omitempty"`
	Body                 string   `protobuf:"bytes,10,opt,name=Body,proto3" json:"Body,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *AppearancePack) Reset()         { *m = AppearancePack{} }
func (m *AppearancePack) String() string { return proto.CompactTextString(m) }
func (*AppearancePack) ProtoMessage()    {}
func (*AppearancePack) Descriptor() ([]byte, []int) {
	return fileDescriptor_87d87b5d90861b2d, []int{4}
}
func (m *AppearancePack) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_AppearancePack.Unmarshal(m, b)
}
func (m *AppearancePack) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_AppearancePack.Marshal(b, m, deterministic)
}
func (m *AppearancePack) XXX_Merge(src proto.Message) {
	xxx_messageInfo_AppearancePack.Merge(m, src)
}
func (m *AppearancePack) XXX_Size() int {
	return xxx_messageInfo_AppearancePack.Size(m)
}
func (m *AppearancePack) XXX_DiscardUnknown() {
	xxx_messageInfo_AppearancePack.DiscardUnknown(m)
}

var xxx_messageInfo_AppearancePack proto.InternalMessageInfo

func (m *AppearancePack) GetHair() string {
	if m != nil {
		return m.Hair
	}
	return ""
}

func (m *AppearancePack) GetFace() string {
	if m != nil {
		return m.Face
	}
	return ""
}

func (m *AppearancePack) GetHead() string {
	if m != nil {
		return m.Head
	}
	return ""
}

func (m *AppearancePack) GetCloth() string {
	if m != nil {
		return m.Cloth
	}
	return ""
}

func (m *AppearancePack) GetPants() string {
	if m != nil {
		return m.Pants
	}
	return ""
}

func (m *AppearancePack) GetArmor() string {
	if m != nil {
		return m.Armor
	}
	return ""
}

func (m *AppearancePack) GetBack() string {
	if m != nil {
		return m.Back
	}
	return ""
}

func (m *AppearancePack) GetRightWeapon() string {
	if m != nil {
		return m.RightWeapon
	}
	return ""
}

func (m *AppearancePack) GetLeftWeapon() string {
	if m != nil {
		return m.LeftWeapon
	}
	return ""
}

func (m *AppearancePack) GetBody() string {
	if m != nil {
		return m.Body
	}
	return ""
}

type PosPack struct {
	PosX                 float32  `protobuf:"fixed32,1,opt,name=PosX,proto3" json:"PosX,omitempty"`
	PosY                 float32  `protobuf:"fixed32,2,opt,name=PosY,proto3" json:"PosY,omitempty"`
	PosZ                 float32  `protobuf:"fixed32,3,opt,name=PosZ,proto3" json:"PosZ,omitempty"`
	RotaX                float32  `protobuf:"fixed32,4,opt,name=RotaX,proto3" json:"RotaX,omitempty"`
	RotaY                float32  `protobuf:"fixed32,5,opt,name=RotaY,proto3" json:"RotaY,omitempty"`
	RotaZ                float32  `protobuf:"fixed32,6,opt,name=RotaZ,proto3" json:"RotaZ,omitempty"`
	GunRotZ              float32  `protobuf:"fixed32,7,opt,name=GunRotZ,proto3" json:"GunRotZ,omitempty"`
	Animation            int32    `protobuf:"varint,8,opt,name=Animation,proto3" json:"Animation,omitempty"`
	Dirt                 float32  `protobuf:"fixed32,9,opt,name=Dirt,proto3" json:"Dirt,omitempty"`
	CtrlLeftRight        float32  `protobuf:"fixed32,10,opt,name=ctrl_leftRight,json=ctrlLeftRight,proto3" json:"ctrl_leftRight,omitempty"`
	CtrlForwardBackward  float32  `protobuf:"fixed32,11,opt,name=ctrl_forwardBackward,json=ctrlForwardBackward,proto3" json:"ctrl_forwardBackward,omitempty"`
	CtrlJump             float32  `protobuf:"fixed32,12,opt,name=ctrl_jump,json=ctrlJump,proto3" json:"ctrl_jump,omitempty"`
	CtrlMouseY           float32  `protobuf:"fixed32,13,opt,name=ctrl_mouseY,json=ctrlMouseY,proto3" json:"ctrl_mouseY,omitempty"`
	CtrlReachLeft        float32  `protobuf:"fixed32,14,opt,name=ctrl_reachLeft,json=ctrlReachLeft,proto3" json:"ctrl_reachLeft,omitempty"`
	CtrlReachRight       float32  `protobuf:"fixed32,15,opt,name=ctrl_reachRight,json=ctrlReachRight,proto3" json:"ctrl_reachRight,omitempty"`
	CtrlPunchLeft        bool     `protobuf:"varint,16,opt,name=ctrl_punchLeft,json=ctrlPunchLeft,proto3" json:"ctrl_punchLeft,omitempty"`
	CtrlPunchRight       bool     `protobuf:"varint,17,opt,name=ctrl_punchRight,json=ctrlPunchRight,proto3" json:"ctrl_punchRight,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *PosPack) Reset()         { *m = PosPack{} }
func (m *PosPack) String() string { return proto.CompactTextString(m) }
func (*PosPack) ProtoMessage()    {}
func (*PosPack) Descriptor() ([]byte, []int) {
	return fileDescriptor_87d87b5d90861b2d, []int{5}
}
func (m *PosPack) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_PosPack.Unmarshal(m, b)
}
func (m *PosPack) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_PosPack.Marshal(b, m, deterministic)
}
func (m *PosPack) XXX_Merge(src proto.Message) {
	xxx_messageInfo_PosPack.Merge(m, src)
}
func (m *PosPack) XXX_Size() int {
	return xxx_messageInfo_PosPack.Size(m)
}
func (m *PosPack) XXX_DiscardUnknown() {
	xxx_messageInfo_PosPack.DiscardUnknown(m)
}

var xxx_messageInfo_PosPack proto.InternalMessageInfo

func (m *PosPack) GetPosX() float32 {
	if m != nil {
		return m.PosX
	}
	return 0
}

func (m *PosPack) GetPosY() float32 {
	if m != nil {
		return m.PosY
	}
	return 0
}

func (m *PosPack) GetPosZ() float32 {
	if m != nil {
		return m.PosZ
	}
	return 0
}

func (m *PosPack) GetRotaX() float32 {
	if m != nil {
		return m.RotaX
	}
	return 0
}

func (m *PosPack) GetRotaY() float32 {
	if m != nil {
		return m.RotaY
	}
	return 0
}

func (m *PosPack) GetRotaZ() float32 {
	if m != nil {
		return m.RotaZ
	}
	return 0
}

func (m *PosPack) GetGunRotZ() float32 {
	if m != nil {
		return m.GunRotZ
	}
	return 0
}

func (m *PosPack) GetAnimation() int32 {
	if m != nil {
		return m.Animation
	}
	return 0
}

func (m *PosPack) GetDirt() float32 {
	if m != nil {
		return m.Dirt
	}
	return 0
}

func (m *PosPack) GetCtrlLeftRight() float32 {
	if m != nil {
		return m.CtrlLeftRight
	}
	return 0
}

func (m *PosPack) GetCtrlForwardBackward() float32 {
	if m != nil {
		return m.CtrlForwardBackward
	}
	return 0
}

func (m *PosPack) GetCtrlJump() float32 {
	if m != nil {
		return m.CtrlJump
	}
	return 0
}

func (m *PosPack) GetCtrlMouseY() float32 {
	if m != nil {
		return m.CtrlMouseY
	}
	return 0
}

func (m *PosPack) GetCtrlReachLeft() float32 {
	if m != nil {
		return m.CtrlReachLeft
	}
	return 0
}

func (m *PosPack) GetCtrlReachRight() float32 {
	if m != nil {
		return m.CtrlReachRight
	}
	return 0
}

func (m *PosPack) GetCtrlPunchLeft() bool {
	if m != nil {
		return m.CtrlPunchLeft
	}
	return false
}

func (m *PosPack) GetCtrlPunchRight() bool {
	if m != nil {
		return m.CtrlPunchRight
	}
	return false
}

func init() {
	proto.RegisterEnum("GameProtocol.RequestCode", RequestCode_name, RequestCode_value)
	proto.RegisterEnum("GameProtocol.ActionCode", ActionCode_name, ActionCode_value)
	proto.RegisterEnum("GameProtocol.ReturnCode", ReturnCode_name, ReturnCode_value)
	proto.RegisterType((*MainPack)(nil), "GameProtocol.MainPack")
	proto.RegisterType((*LoginPack)(nil), "GameProtocol.LoginPack")
	proto.RegisterType((*RoomPack)(nil), "GameProtocol.RoomPack")
	proto.RegisterType((*PlayerPack)(nil), "GameProtocol.PlayerPack")
	proto.RegisterType((*AppearancePack)(nil), "GameProtocol.AppearancePack")
	proto.RegisterType((*PosPack)(nil), "GameProtocol.PosPack")
}

func init() { proto.RegisterFile("Goproto/TGameProtocol.proto", fileDescriptor_87d87b5d90861b2d) }

var fileDescriptor_87d87b5d90861b2d = []byte{
	// 971 bytes of a gzipped FileDescriptorProto
	0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xff, 0x6c, 0x55, 0x5f, 0x6f, 0x1b, 0x45,
	0x10, 0xc7, 0x67, 0x3b, 0xbe, 0x1b, 0x27, 0xce, 0x76, 0x5b, 0xca, 0x41, 0x2b, 0xb0, 0x2c, 0x21,
	0xa2, 0x3c, 0xb4, 0x22, 0x08, 0x09, 0x09, 0x5e, 0x5c, 0x17, 0xa7, 0x44, 0x29, 0xb2, 0x36, 0x44,
	0xd4, 0x7e, 0x41, 0xdb, 0xbb, 0x4d, 0x7c, 0xe0, 0xbb, 0x3d, 0xf6, 0xf6, 0x68, 0xf3, 0x05, 0x78,
	0xe1, 0x53, 0xf0, 0xe1, 0xf8, 0x0e, 0x3c, 0xa2, 0x99, 0xbd, 0x7f, 0x8e, 0x78, 0xca, 0xcc, 0x6f,
	0x7e, 0xf3, 0xdb, 0xf9, 0xcd, 0x5e, 0xd6, 0xf0, 0xe4, 0x5c, 0xe7, 0x46, 0x5b, 0xfd, 0xfc, 0xa7,
	0x73, 0x99, 0xaa, 0x15, 0x86, 0x91, 0xde, 0x3d, 0x23, 0x8c, 0x1f, 0x76, 0xb1, 0xd9, 0x9f, 0x7d,
	0xf0, 0x5f, 0xcb, 0x24, 0x5b, 0xc9, 0xe8, 0x37, 0xfe, 0x2d, 0x8c, 0x8d, 0xfa, 0xbd, 0x54, 0x85,
	0x8d, 0x74, 0xac, 0xc2, 0xde, 0xb4, 0x77, 0x32, 0x39, 0xfb, 0xf8, 0xd9, 0x9e, 0x88, 0x70, 0x84,
	0x85, 0x8e, 0x95, 0xe8, 0xb2, 0xf9, 0x37, 0x00, 0x32, 0xb2, 0x89, 0xce, 0xa8, 0xd7, 0xa3, 0xde,
	0x70, 0xbf, 0x77, 0x4e, 0x75, 0x6a, 0xed, 0x70, 0xb1, 0xd3, 0x28, 0x5b, 0x1a, 0xd7, 0xd9, 0xff,
	0xbf, 0x4e, 0x41, 0x75, 0xd7, 0xd9, 0x72, 0xf9, 0xd7, 0x10, 0xec, 0xf4, 0xad, 0x9b, 0x3e, 0x1c,
	0x4c, 0x7b, 0x27, 0xe3, 0xb3, 0x8f, 0xf6, 0x1b, 0x2f, 0xeb, 0xb2, 0x68, 0x99, 0x9c, 0x41, 0xbf,
	0xb0, 0x26, 0x1c, 0x4e, 0x7b, 0x27, 0x81, 0xc0, 0x90, 0x9f, 0x81, 0x6f, 0xb4, 0x4e, 0x73, 0xd4,
	0x39, 0x98, 0xf6, 0x4f, 0xc6, 0x67, 0x8f, 0xef, 0x0d, 0xa0, 0x75, 0x4a, 0x32, 0x0d, 0x0f, 0xc7,
	0xce, 0x77, 0xf2, 0x4e, 0x19, 0xea, 0x1a, 0x51, 0xd7, 0xbd, 0xb1, 0x57, 0x54, 0xa7, 0xbe, 0x0e,
	0x97, 0x73, 0x18, 0x94, 0x85, 0x32, 0xa1, 0x4f, 0x03, 0x50, 0x3c, 0x5b, 0x40, 0xd0, 0xcc, 0xca,
	0x3f, 0x01, 0x1f, 0xc1, 0x4c, 0xa6, 0xee, 0x16, 0x02, 0xd1, 0xe4, 0x58, 0xcb, 0x65, 0x51, 0xbc,
	0xd3, 0x26, 0xa6, 0x2d, 0x07, 0xa2, 0xc9, 0x67, 0x3b, 0xf0, 0xeb, 0x41, 0x91, 0x87, 0xa3, 0x76,
	0x35, 0xea, 0x9c, 0x3f, 0x86, 0x83, 0x54, 0xbe, 0xcf, 0xca, 0x94, 0x14, 0x86, 0xa2, 0xca, 0x10,
	0x8f, 0x4a, 0x83, 0x78, 0xdf, 0xe1, 0x2e, 0xe3, 0x8f, 0x60, 0x58, 0x58, 0x69, 0x15, 0xed, 0x78,
	0x28, 0x5c, 0x32, 0xfb, 0xa7, 0x07, 0xd0, 0x3a, 0xe4, 0x9f, 0xd6, 0xfb, 0xe8, 0x1c, 0xd9, 0x41,
	0x68, 0x70, 0xca, 0x7e, 0x78, 0xd9, 0x0c, 0x5e, 0xe5, 0x7c, 0x02, 0xde, 0x36, 0xaf, 0x0e, 0xf5,
	0xb6, 0x39, 0x7f, 0x0e, 0xa3, 0x5c, 0x17, 0x9d, 0x6b, 0xfd, 0xf0, 0xde, 0x62, 0x5d, 0x51, 0xd4,
	0x2c, 0xfe, 0x14, 0x02, 0x27, 0x76, 0xa1, 0xdf, 0xd2, 0xc5, 0x0e, 0x45, 0x0b, 0xf0, 0xef, 0x00,
	0x64, 0x9e, 0x2b, 0x69, 0x64, 0x16, 0xa9, 0xf0, 0x80, 0x14, 0x9f, 0xde, 0xfb, 0x36, 0x9b, 0xba,
	0xbb, 0xae, 0x96, 0x3f, 0xfb, 0xb7, 0x07, 0x93, 0xfd, 0x32, 0xde, 0xe0, 0x2b, 0x99, 0x98, 0xca,
	0x25, 0xc5, 0x88, 0x2d, 0x65, 0xa4, 0x2a, 0x6f, 0x14, 0x13, 0x4f, 0xc9, 0x98, 0x9c, 0x21, 0x4f,
	0xc9, 0x18, 0x97, 0xb9, 0xd8, 0x69, 0xbb, 0x25, 0x67, 0x81, 0x70, 0x09, 0xa2, 0x2b, 0x99, 0xd9,
	0xa2, 0xfa, 0x2a, 0x5d, 0x82, 0xe8, 0xdc, 0xa4, 0xda, 0xd0, 0xcc, 0x81, 0x70, 0x09, 0xaa, 0xbe,
	0x70, 0xdf, 0x1c, 0xa9, 0x62, 0xcc, 0xa7, 0x30, 0x16, 0xc9, 0xed, 0xd6, 0xfe, 0xac, 0x64, 0xae,
	0xb3, 0xea, 0xd3, 0xea, 0x42, 0x78, 0x3f, 0x97, 0xea, 0xa6, 0x26, 0x04, 0xee, 0x7e, 0x5a, 0x84,
	0x54, 0x75, 0x7c, 0x17, 0x42, 0xa5, 0xaa, 0xe3, 0xbb, 0xd9, 0x5f, 0x03, 0x18, 0x55, 0xbb, 0xc6,
	0xfa, 0x4a, 0x17, 0x6f, 0xc8, 0xb3, 0x27, 0x28, 0xae, 0xb0, 0x35, 0x79, 0x76, 0xd8, 0xba, 0xc2,
	0x36, 0xe4, 0xd9, 0x61, 0x1b, 0xf4, 0x21, 0xb4, 0x95, 0x6f, 0xc8, 0xb3, 0x27, 0x5c, 0x52, 0xa3,
	0x6b, 0xf2, 0x5c, 0xa1, 0xeb, 0x1a, 0xdd, 0x90, 0xe7, 0x0a, 0xdd, 0xf0, 0x10, 0x46, 0xe7, 0x65,
	0x26, 0xb4, 0xdd, 0x90, 0x6d, 0x4f, 0xd4, 0x29, 0x5e, 0xfd, 0x3c, 0x4b, 0x52, 0x89, 0xef, 0x09,
	0xf9, 0x1e, 0x8a, 0x16, 0xc0, 0x69, 0x5e, 0x26, 0xc6, 0x92, 0x5f, 0x4f, 0x50, 0xcc, 0x3f, 0x87,
	0x49, 0x64, 0xcd, 0xee, 0x97, 0x9d, 0xba, 0xb1, 0xb4, 0x21, 0xf2, 0xec, 0x89, 0x23, 0x44, 0x2f,
	0x6b, 0x90, 0x7f, 0x09, 0x8f, 0x88, 0x76, 0xa3, 0xcd, 0x3b, 0x69, 0x62, 0x5c, 0x33, 0xfe, 0x0d,
	0xc7, 0x44, 0x7e, 0x88, 0xb5, 0xe5, 0x7e, 0x89, 0x3f, 0x81, 0x80, 0x5a, 0x7e, 0x2d, 0xd3, 0x3c,
	0x3c, 0x24, 0x9e, 0x8f, 0xc0, 0x45, 0x99, 0xe6, 0xfc, 0x33, 0x18, 0x53, 0x31, 0xd5, 0x65, 0xa1,
	0xd6, 0xe1, 0x11, 0x95, 0x01, 0xa1, 0xd7, 0x84, 0x34, 0x73, 0x19, 0x25, 0xa3, 0x2d, 0xce, 0x11,
	0x4e, 0xda, 0xb9, 0x44, 0x0d, 0xf2, 0x2f, 0xe0, 0xb8, 0xa5, 0xb9, 0xf9, 0x8f, 0x89, 0x37, 0x69,
	0x78, 0xce, 0x40, 0xad, 0x97, 0x97, 0x59, 0xa5, 0xc7, 0xa6, 0xbd, 0x13, 0xdf, 0xe9, 0xad, 0x6a,
	0xb0, 0xd1, 0x23, 0x9a, 0xd3, 0x7b, 0x40, 0xbc, 0x49, 0xc3, 0x23, 0xf4, 0xf4, 0x1c, 0xc6, 0x9d,
	0xe7, 0x9f, 0x1f, 0x37, 0xe9, 0x8f, 0x3a, 0x53, 0xec, 0x03, 0xee, 0xc3, 0xe0, 0xba, 0x50, 0x86,
	0xf5, 0x30, 0xc2, 0x87, 0x88, 0x79, 0x18, 0xe1, 0xff, 0x19, 0xeb, 0xf3, 0x00, 0x86, 0xaf, 0x94,
	0x34, 0x96, 0x0d, 0x4e, 0xff, 0xf6, 0x00, 0xda, 0x1f, 0x03, 0x3e, 0xa9, 0xb3, 0x4a, 0xe7, 0x10,
	0x7c, 0xa1, 0x6e, 0x93, 0xc2, 0x92, 0x56, 0x00, 0x43, 0x7a, 0x19, 0x99, 0x87, 0xc4, 0x85, 0x51,
	0xd2, 0x2a, 0x12, 0xef, 0x23, 0x71, 0x99, 0x64, 0x31, 0x65, 0x03, 0xac, 0xba, 0xe7, 0xe8, 0x32,
	0x29, 0x2c, 0x1b, 0x62, 0xf5, 0x42, 0x27, 0x19, 0x55, 0x0f, 0x70, 0x90, 0xef, 0xdf, 0x27, 0x96,
	0x8d, 0x30, 0x5a, 0x6c, 0xa5, 0x65, 0x3e, 0x3f, 0x82, 0xe0, 0xca, 0x4a, 0x63, 0x69, 0xc2, 0x00,
	0x1b, 0x28, 0x4d, 0xb2, 0x5b, 0x06, 0x68, 0xef, 0x3a, 0x8f, 0xa5, 0x55, 0x57, 0xf8, 0xda, 0xb1,
	0x31, 0x96, 0x51, 0x81, 0xc8, 0x87, 0xfc, 0x21, 0x1c, 0x5f, 0xe7, 0x8b, 0xad, 0x34, 0x32, 0xb2,
	0xd5, 0x91, 0x47, 0x38, 0xeb, 0x75, 0xbe, 0xd2, 0x05, 0x9b, 0xe0, 0x29, 0xcb, 0xc4, 0x28, 0x76,
	0x8c, 0xa7, 0x90, 0xf1, 0x17, 0x4a, 0x5a, 0xc6, 0x38, 0x83, 0xc3, 0x79, 0x1c, 0x37, 0x9d, 0xec,
	0x01, 0x4a, 0x09, 0x95, 0xea, 0x3f, 0x54, 0x0b, 0xf2, 0xd3, 0x39, 0x40, 0xfb, 0xab, 0x87, 0xde,
	0x5c, 0x56, 0xad, 0x68, 0x0c, 0xa3, 0xab, 0x32, 0x8a, 0x54, 0x51, 0xb8, 0x6d, 0x2f, 0x65, 0xb2,
	0x63, 0x1e, 0x8e, 0x88, 0x04, 0xb7, 0x9e, 0xb7, 0x07, 0xf4, 0x8b, 0xff, 0xd5, 0x7f, 0x01, 0x00,
	0x00, 0xff, 0xff, 0xcf, 0xf2, 0x29, 0x0b, 0x10, 0x08, 0x00, 0x00,
}
