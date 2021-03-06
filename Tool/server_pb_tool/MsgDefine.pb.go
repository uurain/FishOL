// Code generated by protoc-gen-go. DO NOT EDIT.
// source: MsgDefine.proto

package msg

import proto "github.com/golang/protobuf/proto"
import fmt "fmt"
import math "math"

// Reference imports to suppress errors if they are not otherwise used.
var _ = proto.Marshal
var _ = fmt.Errorf
var _ = math.Inf

// This is a compile-time assertion to ensure that this generated file
// is compatible with the proto package it is being compiled against.
// A compilation error at this line likely means your copy of the
// proto package needs to be updated.
const _ = proto.ProtoPackageIsVersion2 // please upgrade the proto package

type EGameEventCode int32

const (
	EGameEventCode_Code_Not_Enough_Gold EGameEventCode = 0
	EGameEventCode_Code_Full_Room       EGameEventCode = 1
)

var EGameEventCode_name = map[int32]string{
	0: "Code_Not_Enough_Gold",
	1: "Code_Full_Room",
}
var EGameEventCode_value = map[string]int32{
	"Code_Not_Enough_Gold": 0,
	"Code_Full_Room":       1,
}

func (x EGameEventCode) Enum() *EGameEventCode {
	p := new(EGameEventCode)
	*p = x
	return p
}
func (x EGameEventCode) String() string {
	return proto.EnumName(EGameEventCode_name, int32(x))
}
func (x *EGameEventCode) UnmarshalJSON(data []byte) error {
	value, err := proto.UnmarshalJSONEnum(EGameEventCode_value, data, "EGameEventCode")
	if err != nil {
		return err
	}
	*x = EGameEventCode(value)
	return nil
}
func (EGameEventCode) EnumDescriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{0}
}

type Vector2 struct {
	X                    *float32 `protobuf:"fixed32,1,req,name=x" json:"x,omitempty"`
	Y                    *float32 `protobuf:"fixed32,2,req,name=y" json:"y,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *Vector2) Reset()         { *m = Vector2{} }
func (m *Vector2) String() string { return proto.CompactTextString(m) }
func (*Vector2) ProtoMessage()    {}
func (*Vector2) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{0}
}
func (m *Vector2) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_Vector2.Unmarshal(m, b)
}
func (m *Vector2) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_Vector2.Marshal(b, m, deterministic)
}
func (dst *Vector2) XXX_Merge(src proto.Message) {
	xxx_messageInfo_Vector2.Merge(dst, src)
}
func (m *Vector2) XXX_Size() int {
	return xxx_messageInfo_Vector2.Size(m)
}
func (m *Vector2) XXX_DiscardUnknown() {
	xxx_messageInfo_Vector2.DiscardUnknown(m)
}

var xxx_messageInfo_Vector2 proto.InternalMessageInfo

func (m *Vector2) GetX() float32 {
	if m != nil && m.X != nil {
		return *m.X
	}
	return 0
}

func (m *Vector2) GetY() float32 {
	if m != nil && m.Y != nil {
		return *m.Y
	}
	return 0
}

type ReqLogin struct {
	AccountId            *string  `protobuf:"bytes,1,req,name=accountId" json:"accountId,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *ReqLogin) Reset()         { *m = ReqLogin{} }
func (m *ReqLogin) String() string { return proto.CompactTextString(m) }
func (*ReqLogin) ProtoMessage()    {}
func (*ReqLogin) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{1}
}
func (m *ReqLogin) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_ReqLogin.Unmarshal(m, b)
}
func (m *ReqLogin) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_ReqLogin.Marshal(b, m, deterministic)
}
func (dst *ReqLogin) XXX_Merge(src proto.Message) {
	xxx_messageInfo_ReqLogin.Merge(dst, src)
}
func (m *ReqLogin) XXX_Size() int {
	return xxx_messageInfo_ReqLogin.Size(m)
}
func (m *ReqLogin) XXX_DiscardUnknown() {
	xxx_messageInfo_ReqLogin.DiscardUnknown(m)
}

var xxx_messageInfo_ReqLogin proto.InternalMessageInfo

func (m *ReqLogin) GetAccountId() string {
	if m != nil && m.AccountId != nil {
		return *m.AccountId
	}
	return ""
}

type AckLogin struct {
	ErrorCode            *int32   `protobuf:"varint,1,req,name=errorCode" json:"errorCode,omitempty"`
	Uid                  *int32   `protobuf:"varint,2,opt,name=uid" json:"uid,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *AckLogin) Reset()         { *m = AckLogin{} }
func (m *AckLogin) String() string { return proto.CompactTextString(m) }
func (*AckLogin) ProtoMessage()    {}
func (*AckLogin) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{2}
}
func (m *AckLogin) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_AckLogin.Unmarshal(m, b)
}
func (m *AckLogin) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_AckLogin.Marshal(b, m, deterministic)
}
func (dst *AckLogin) XXX_Merge(src proto.Message) {
	xxx_messageInfo_AckLogin.Merge(dst, src)
}
func (m *AckLogin) XXX_Size() int {
	return xxx_messageInfo_AckLogin.Size(m)
}
func (m *AckLogin) XXX_DiscardUnknown() {
	xxx_messageInfo_AckLogin.DiscardUnknown(m)
}

var xxx_messageInfo_AckLogin proto.InternalMessageInfo

func (m *AckLogin) GetErrorCode() int32 {
	if m != nil && m.ErrorCode != nil {
		return *m.ErrorCode
	}
	return 0
}

func (m *AckLogin) GetUid() int32 {
	if m != nil && m.Uid != nil {
		return *m.Uid
	}
	return 0
}

type ReqAckEnterRoom struct {
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *ReqAckEnterRoom) Reset()         { *m = ReqAckEnterRoom{} }
func (m *ReqAckEnterRoom) String() string { return proto.CompactTextString(m) }
func (*ReqAckEnterRoom) ProtoMessage()    {}
func (*ReqAckEnterRoom) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{3}
}
func (m *ReqAckEnterRoom) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_ReqAckEnterRoom.Unmarshal(m, b)
}
func (m *ReqAckEnterRoom) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_ReqAckEnterRoom.Marshal(b, m, deterministic)
}
func (dst *ReqAckEnterRoom) XXX_Merge(src proto.Message) {
	xxx_messageInfo_ReqAckEnterRoom.Merge(dst, src)
}
func (m *ReqAckEnterRoom) XXX_Size() int {
	return xxx_messageInfo_ReqAckEnterRoom.Size(m)
}
func (m *ReqAckEnterRoom) XXX_DiscardUnknown() {
	xxx_messageInfo_ReqAckEnterRoom.DiscardUnknown(m)
}

var xxx_messageInfo_ReqAckEnterRoom proto.InternalMessageInfo

type ReqAckLeaveRoom struct {
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *ReqAckLeaveRoom) Reset()         { *m = ReqAckLeaveRoom{} }
func (m *ReqAckLeaveRoom) String() string { return proto.CompactTextString(m) }
func (*ReqAckLeaveRoom) ProtoMessage()    {}
func (*ReqAckLeaveRoom) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{4}
}
func (m *ReqAckLeaveRoom) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_ReqAckLeaveRoom.Unmarshal(m, b)
}
func (m *ReqAckLeaveRoom) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_ReqAckLeaveRoom.Marshal(b, m, deterministic)
}
func (dst *ReqAckLeaveRoom) XXX_Merge(src proto.Message) {
	xxx_messageInfo_ReqAckLeaveRoom.Merge(dst, src)
}
func (m *ReqAckLeaveRoom) XXX_Size() int {
	return xxx_messageInfo_ReqAckLeaveRoom.Size(m)
}
func (m *ReqAckLeaveRoom) XXX_DiscardUnknown() {
	xxx_messageInfo_ReqAckLeaveRoom.DiscardUnknown(m)
}

var xxx_messageInfo_ReqAckLeaveRoom proto.InternalMessageInfo

type PlayerInfo struct {
	Gold                 *int32   `protobuf:"varint,6,opt,name=gold" json:"gold,omitempty"`
	Name                 *string  `protobuf:"bytes,13,opt,name=name" json:"name,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *PlayerInfo) Reset()         { *m = PlayerInfo{} }
func (m *PlayerInfo) String() string { return proto.CompactTextString(m) }
func (*PlayerInfo) ProtoMessage()    {}
func (*PlayerInfo) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{5}
}
func (m *PlayerInfo) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_PlayerInfo.Unmarshal(m, b)
}
func (m *PlayerInfo) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_PlayerInfo.Marshal(b, m, deterministic)
}
func (dst *PlayerInfo) XXX_Merge(src proto.Message) {
	xxx_messageInfo_PlayerInfo.Merge(dst, src)
}
func (m *PlayerInfo) XXX_Size() int {
	return xxx_messageInfo_PlayerInfo.Size(m)
}
func (m *PlayerInfo) XXX_DiscardUnknown() {
	xxx_messageInfo_PlayerInfo.DiscardUnknown(m)
}

var xxx_messageInfo_PlayerInfo proto.InternalMessageInfo

func (m *PlayerInfo) GetGold() int32 {
	if m != nil && m.Gold != nil {
		return *m.Gold
	}
	return 0
}

func (m *PlayerInfo) GetName() string {
	if m != nil && m.Name != nil {
		return *m.Name
	}
	return ""
}

type PlayerEntryInfo struct {
	Uid                  *int32      `protobuf:"varint,1,req,name=uid" json:"uid,omitempty"`
	TableIndex           *int32      `protobuf:"varint,2,req,name=table_index" json:"table_index,omitempty"`
	PlayerInfo           *PlayerInfo `protobuf:"bytes,6,req,name=player_info" json:"player_info,omitempty"`
	XXX_NoUnkeyedLiteral struct{}    `json:"-"`
	XXX_unrecognized     []byte      `json:"-"`
	XXX_sizecache        int32       `json:"-"`
}

func (m *PlayerEntryInfo) Reset()         { *m = PlayerEntryInfo{} }
func (m *PlayerEntryInfo) String() string { return proto.CompactTextString(m) }
func (*PlayerEntryInfo) ProtoMessage()    {}
func (*PlayerEntryInfo) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{6}
}
func (m *PlayerEntryInfo) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_PlayerEntryInfo.Unmarshal(m, b)
}
func (m *PlayerEntryInfo) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_PlayerEntryInfo.Marshal(b, m, deterministic)
}
func (dst *PlayerEntryInfo) XXX_Merge(src proto.Message) {
	xxx_messageInfo_PlayerEntryInfo.Merge(dst, src)
}
func (m *PlayerEntryInfo) XXX_Size() int {
	return xxx_messageInfo_PlayerEntryInfo.Size(m)
}
func (m *PlayerEntryInfo) XXX_DiscardUnknown() {
	xxx_messageInfo_PlayerEntryInfo.DiscardUnknown(m)
}

var xxx_messageInfo_PlayerEntryInfo proto.InternalMessageInfo

func (m *PlayerEntryInfo) GetUid() int32 {
	if m != nil && m.Uid != nil {
		return *m.Uid
	}
	return 0
}

func (m *PlayerEntryInfo) GetTableIndex() int32 {
	if m != nil && m.TableIndex != nil {
		return *m.TableIndex
	}
	return 0
}

func (m *PlayerEntryInfo) GetPlayerInfo() *PlayerInfo {
	if m != nil {
		return m.PlayerInfo
	}
	return nil
}

type AckPlayerEntryList struct {
	ObjectList           []*PlayerEntryInfo `protobuf:"bytes,1,rep,name=object_list" json:"object_list,omitempty"`
	XXX_NoUnkeyedLiteral struct{}           `json:"-"`
	XXX_unrecognized     []byte             `json:"-"`
	XXX_sizecache        int32              `json:"-"`
}

func (m *AckPlayerEntryList) Reset()         { *m = AckPlayerEntryList{} }
func (m *AckPlayerEntryList) String() string { return proto.CompactTextString(m) }
func (*AckPlayerEntryList) ProtoMessage()    {}
func (*AckPlayerEntryList) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{7}
}
func (m *AckPlayerEntryList) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_AckPlayerEntryList.Unmarshal(m, b)
}
func (m *AckPlayerEntryList) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_AckPlayerEntryList.Marshal(b, m, deterministic)
}
func (dst *AckPlayerEntryList) XXX_Merge(src proto.Message) {
	xxx_messageInfo_AckPlayerEntryList.Merge(dst, src)
}
func (m *AckPlayerEntryList) XXX_Size() int {
	return xxx_messageInfo_AckPlayerEntryList.Size(m)
}
func (m *AckPlayerEntryList) XXX_DiscardUnknown() {
	xxx_messageInfo_AckPlayerEntryList.DiscardUnknown(m)
}

var xxx_messageInfo_AckPlayerEntryList proto.InternalMessageInfo

func (m *AckPlayerEntryList) GetObjectList() []*PlayerEntryInfo {
	if m != nil {
		return m.ObjectList
	}
	return nil
}

type AckPlayerLeaveList struct {
	ObjectList           []int32  `protobuf:"varint,1,rep,name=object_list" json:"object_list,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *AckPlayerLeaveList) Reset()         { *m = AckPlayerLeaveList{} }
func (m *AckPlayerLeaveList) String() string { return proto.CompactTextString(m) }
func (*AckPlayerLeaveList) ProtoMessage()    {}
func (*AckPlayerLeaveList) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{8}
}
func (m *AckPlayerLeaveList) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_AckPlayerLeaveList.Unmarshal(m, b)
}
func (m *AckPlayerLeaveList) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_AckPlayerLeaveList.Marshal(b, m, deterministic)
}
func (dst *AckPlayerLeaveList) XXX_Merge(src proto.Message) {
	xxx_messageInfo_AckPlayerLeaveList.Merge(dst, src)
}
func (m *AckPlayerLeaveList) XXX_Size() int {
	return xxx_messageInfo_AckPlayerLeaveList.Size(m)
}
func (m *AckPlayerLeaveList) XXX_DiscardUnknown() {
	xxx_messageInfo_AckPlayerLeaveList.DiscardUnknown(m)
}

var xxx_messageInfo_AckPlayerLeaveList proto.InternalMessageInfo

func (m *AckPlayerLeaveList) GetObjectList() []int32 {
	if m != nil {
		return m.ObjectList
	}
	return nil
}

// 属性同步
type AckPublicPropertyList struct {
	Uid                  *int32      `protobuf:"varint,1,req,name=uid" json:"uid,omitempty"`
	PlayerInfo           *PlayerInfo `protobuf:"bytes,2,req,name=player_info" json:"player_info,omitempty"`
	XXX_NoUnkeyedLiteral struct{}    `json:"-"`
	XXX_unrecognized     []byte      `json:"-"`
	XXX_sizecache        int32       `json:"-"`
}

func (m *AckPublicPropertyList) Reset()         { *m = AckPublicPropertyList{} }
func (m *AckPublicPropertyList) String() string { return proto.CompactTextString(m) }
func (*AckPublicPropertyList) ProtoMessage()    {}
func (*AckPublicPropertyList) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{9}
}
func (m *AckPublicPropertyList) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_AckPublicPropertyList.Unmarshal(m, b)
}
func (m *AckPublicPropertyList) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_AckPublicPropertyList.Marshal(b, m, deterministic)
}
func (dst *AckPublicPropertyList) XXX_Merge(src proto.Message) {
	xxx_messageInfo_AckPublicPropertyList.Merge(dst, src)
}
func (m *AckPublicPropertyList) XXX_Size() int {
	return xxx_messageInfo_AckPublicPropertyList.Size(m)
}
func (m *AckPublicPropertyList) XXX_DiscardUnknown() {
	xxx_messageInfo_AckPublicPropertyList.DiscardUnknown(m)
}

var xxx_messageInfo_AckPublicPropertyList proto.InternalMessageInfo

func (m *AckPublicPropertyList) GetUid() int32 {
	if m != nil && m.Uid != nil {
		return *m.Uid
	}
	return 0
}

func (m *AckPublicPropertyList) GetPlayerInfo() *PlayerInfo {
	if m != nil {
		return m.PlayerInfo
	}
	return nil
}

type ReqAckBullet struct {
	Uid                  *int32   `protobuf:"varint,1,req,name=uid" json:"uid,omitempty"`
	BulletType           *int32   `protobuf:"varint,2,req,name=bullet_type" json:"bullet_type,omitempty"`
	Tpos                 *Vector2 `protobuf:"bytes,3,req,name=tpos" json:"tpos,omitempty"`
	SPos                 *Vector2 `protobuf:"bytes,4,req,name=sPos" json:"sPos,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *ReqAckBullet) Reset()         { *m = ReqAckBullet{} }
func (m *ReqAckBullet) String() string { return proto.CompactTextString(m) }
func (*ReqAckBullet) ProtoMessage()    {}
func (*ReqAckBullet) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{10}
}
func (m *ReqAckBullet) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_ReqAckBullet.Unmarshal(m, b)
}
func (m *ReqAckBullet) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_ReqAckBullet.Marshal(b, m, deterministic)
}
func (dst *ReqAckBullet) XXX_Merge(src proto.Message) {
	xxx_messageInfo_ReqAckBullet.Merge(dst, src)
}
func (m *ReqAckBullet) XXX_Size() int {
	return xxx_messageInfo_ReqAckBullet.Size(m)
}
func (m *ReqAckBullet) XXX_DiscardUnknown() {
	xxx_messageInfo_ReqAckBullet.DiscardUnknown(m)
}

var xxx_messageInfo_ReqAckBullet proto.InternalMessageInfo

func (m *ReqAckBullet) GetUid() int32 {
	if m != nil && m.Uid != nil {
		return *m.Uid
	}
	return 0
}

func (m *ReqAckBullet) GetBulletType() int32 {
	if m != nil && m.BulletType != nil {
		return *m.BulletType
	}
	return 0
}

func (m *ReqAckBullet) GetTpos() *Vector2 {
	if m != nil {
		return m.Tpos
	}
	return nil
}

func (m *ReqAckBullet) GetSPos() *Vector2 {
	if m != nil {
		return m.SPos
	}
	return nil
}

type AckFishOpt struct {
	FishId               *int32   `protobuf:"varint,1,req,name=fish_id" json:"fish_id,omitempty"`
	OptType              *int32   `protobuf:"varint,2,req,name=opt_type" json:"opt_type,omitempty"`
	PathId               *int32   `protobuf:"varint,3,opt,name=path_id" json:"path_id,omitempty"`
	FishConfigId         *int32   `protobuf:"varint,4,opt,name=fishConfigId" json:"fishConfigId,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *AckFishOpt) Reset()         { *m = AckFishOpt{} }
func (m *AckFishOpt) String() string { return proto.CompactTextString(m) }
func (*AckFishOpt) ProtoMessage()    {}
func (*AckFishOpt) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{11}
}
func (m *AckFishOpt) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_AckFishOpt.Unmarshal(m, b)
}
func (m *AckFishOpt) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_AckFishOpt.Marshal(b, m, deterministic)
}
func (dst *AckFishOpt) XXX_Merge(src proto.Message) {
	xxx_messageInfo_AckFishOpt.Merge(dst, src)
}
func (m *AckFishOpt) XXX_Size() int {
	return xxx_messageInfo_AckFishOpt.Size(m)
}
func (m *AckFishOpt) XXX_DiscardUnknown() {
	xxx_messageInfo_AckFishOpt.DiscardUnknown(m)
}

var xxx_messageInfo_AckFishOpt proto.InternalMessageInfo

func (m *AckFishOpt) GetFishId() int32 {
	if m != nil && m.FishId != nil {
		return *m.FishId
	}
	return 0
}

func (m *AckFishOpt) GetOptType() int32 {
	if m != nil && m.OptType != nil {
		return *m.OptType
	}
	return 0
}

func (m *AckFishOpt) GetPathId() int32 {
	if m != nil && m.PathId != nil {
		return *m.PathId
	}
	return 0
}

func (m *AckFishOpt) GetFishConfigId() int32 {
	if m != nil && m.FishConfigId != nil {
		return *m.FishConfigId
	}
	return 0
}

// 各种失败
type AckError struct {
	ErrorCode            *int32   `protobuf:"varint,1,req,name=error_code" json:"error_code,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *AckError) Reset()         { *m = AckError{} }
func (m *AckError) String() string { return proto.CompactTextString(m) }
func (*AckError) ProtoMessage()    {}
func (*AckError) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{12}
}
func (m *AckError) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_AckError.Unmarshal(m, b)
}
func (m *AckError) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_AckError.Marshal(b, m, deterministic)
}
func (dst *AckError) XXX_Merge(src proto.Message) {
	xxx_messageInfo_AckError.Merge(dst, src)
}
func (m *AckError) XXX_Size() int {
	return xxx_messageInfo_AckError.Size(m)
}
func (m *AckError) XXX_DiscardUnknown() {
	xxx_messageInfo_AckError.DiscardUnknown(m)
}

var xxx_messageInfo_AckError proto.InternalMessageInfo

func (m *AckError) GetErrorCode() int32 {
	if m != nil && m.ErrorCode != nil {
		return *m.ErrorCode
	}
	return 0
}

type ReqAckHitFish struct {
	Uid                  *int32   `protobuf:"varint,1,req,name=uid" json:"uid,omitempty"`
	FishId               *int32   `protobuf:"varint,2,req,name=fish_id" json:"fish_id,omitempty"`
	RewardGold           *int32   `protobuf:"varint,3,opt,name=reward_gold" json:"reward_gold,omitempty"`
	XXX_NoUnkeyedLiteral struct{} `json:"-"`
	XXX_unrecognized     []byte   `json:"-"`
	XXX_sizecache        int32    `json:"-"`
}

func (m *ReqAckHitFish) Reset()         { *m = ReqAckHitFish{} }
func (m *ReqAckHitFish) String() string { return proto.CompactTextString(m) }
func (*ReqAckHitFish) ProtoMessage()    {}
func (*ReqAckHitFish) Descriptor() ([]byte, []int) {
	return fileDescriptor_MsgDefine_425c779b709fed8c, []int{13}
}
func (m *ReqAckHitFish) XXX_Unmarshal(b []byte) error {
	return xxx_messageInfo_ReqAckHitFish.Unmarshal(m, b)
}
func (m *ReqAckHitFish) XXX_Marshal(b []byte, deterministic bool) ([]byte, error) {
	return xxx_messageInfo_ReqAckHitFish.Marshal(b, m, deterministic)
}
func (dst *ReqAckHitFish) XXX_Merge(src proto.Message) {
	xxx_messageInfo_ReqAckHitFish.Merge(dst, src)
}
func (m *ReqAckHitFish) XXX_Size() int {
	return xxx_messageInfo_ReqAckHitFish.Size(m)
}
func (m *ReqAckHitFish) XXX_DiscardUnknown() {
	xxx_messageInfo_ReqAckHitFish.DiscardUnknown(m)
}

var xxx_messageInfo_ReqAckHitFish proto.InternalMessageInfo

func (m *ReqAckHitFish) GetUid() int32 {
	if m != nil && m.Uid != nil {
		return *m.Uid
	}
	return 0
}

func (m *ReqAckHitFish) GetFishId() int32 {
	if m != nil && m.FishId != nil {
		return *m.FishId
	}
	return 0
}

func (m *ReqAckHitFish) GetRewardGold() int32 {
	if m != nil && m.RewardGold != nil {
		return *m.RewardGold
	}
	return 0
}

func init() {
	proto.RegisterType((*Vector2)(nil), "msg.Vector2")
	proto.RegisterType((*ReqLogin)(nil), "msg.ReqLogin")
	proto.RegisterType((*AckLogin)(nil), "msg.AckLogin")
	proto.RegisterType((*ReqAckEnterRoom)(nil), "msg.ReqAckEnterRoom")
	proto.RegisterType((*ReqAckLeaveRoom)(nil), "msg.ReqAckLeaveRoom")
	proto.RegisterType((*PlayerInfo)(nil), "msg.PlayerInfo")
	proto.RegisterType((*PlayerEntryInfo)(nil), "msg.PlayerEntryInfo")
	proto.RegisterType((*AckPlayerEntryList)(nil), "msg.AckPlayerEntryList")
	proto.RegisterType((*AckPlayerLeaveList)(nil), "msg.AckPlayerLeaveList")
	proto.RegisterType((*AckPublicPropertyList)(nil), "msg.AckPublicPropertyList")
	proto.RegisterType((*ReqAckBullet)(nil), "msg.ReqAckBullet")
	proto.RegisterType((*AckFishOpt)(nil), "msg.AckFishOpt")
	proto.RegisterType((*AckError)(nil), "msg.AckError")
	proto.RegisterType((*ReqAckHitFish)(nil), "msg.ReqAckHitFish")
	proto.RegisterEnum("msg.EGameEventCode", EGameEventCode_name, EGameEventCode_value)
}

func init() { proto.RegisterFile("MsgDefine.proto", fileDescriptor_MsgDefine_425c779b709fed8c) }

var fileDescriptor_MsgDefine_425c779b709fed8c = []byte{
	// 502 bytes of a gzipped FileDescriptorProto
	0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xff, 0x6c, 0x52, 0xc1, 0x6e, 0xda, 0x40,
	0x10, 0xad, 0x0d, 0x24, 0x30, 0x26, 0x31, 0xd9, 0x50, 0x69, 0x15, 0xa9, 0x2d, 0xb2, 0x7a, 0x70,
	0x72, 0xe0, 0xc0, 0x07, 0xb4, 0x22, 0xa9, 0x93, 0x52, 0xd1, 0x16, 0x71, 0x88, 0xd4, 0xd3, 0xca,
	0xd8, 0x8b, 0xd9, 0x62, 0x76, 0x9c, 0xf5, 0x3a, 0x0d, 0x7f, 0x5f, 0xed, 0x92, 0x10, 0x28, 0x3d,
	0xb1, 0xcc, 0xbc, 0x79, 0x6f, 0xde, 0xf8, 0x81, 0xff, 0xbd, 0xcc, 0xbe, 0xf0, 0xb9, 0x90, 0xbc,
	0x5f, 0x28, 0xd4, 0x48, 0x6a, 0xab, 0x32, 0x0b, 0x3e, 0xc0, 0xf1, 0x3d, 0x4f, 0x34, 0xaa, 0x01,
	0x69, 0x81, 0xf3, 0x44, 0x9d, 0x9e, 0x1b, 0xba, 0xe6, 0xb9, 0xa6, 0xae, 0x79, 0x06, 0xef, 0xa0,
	0x39, 0xe5, 0x0f, 0x63, 0xcc, 0x84, 0x24, 0x67, 0xd0, 0x8a, 0x93, 0x04, 0x2b, 0xa9, 0x47, 0xa9,
	0x45, 0xb6, 0x82, 0x2b, 0x68, 0x0e, 0x93, 0xe5, 0xb6, 0xcd, 0x95, 0x42, 0x75, 0x83, 0x29, 0xb7,
	0xed, 0x06, 0xf1, 0xa0, 0x56, 0x89, 0x94, 0xba, 0x3d, 0x27, 0x6c, 0x04, 0x67, 0xe0, 0x4f, 0xf9,
	0xc3, 0x30, 0x59, 0x46, 0x52, 0x73, 0x35, 0x45, 0x5c, 0xbd, 0x96, 0xc6, 0x3c, 0x7e, 0xe4, 0xb6,
	0x14, 0x02, 0x4c, 0xf2, 0x78, 0xcd, 0xd5, 0x48, 0xce, 0x91, 0xb4, 0xa1, 0x9e, 0x61, 0x9e, 0xd2,
	0x23, 0xc3, 0x60, 0xfe, 0xc9, 0x78, 0xc5, 0xe9, 0x49, 0xcf, 0x09, 0x5b, 0xc1, 0x2f, 0xf0, 0x37,
	0xc8, 0x48, 0x6a, 0xb5, 0xb6, 0xf0, 0x67, 0xbd, 0x8d, 0xf8, 0x39, 0x78, 0x3a, 0x9e, 0xe5, 0x9c,
	0x09, 0x99, 0xf2, 0x27, 0xeb, 0xa7, 0x41, 0x3e, 0x82, 0x57, 0xd8, 0x21, 0x26, 0xe4, 0x1c, 0xe9,
	0x51, 0xcf, 0x0d, 0xbd, 0x81, 0xdf, 0x5f, 0x95, 0x59, 0xff, 0x55, 0x36, 0xf8, 0x0c, 0x64, 0x98,
	0x2c, 0x77, 0xd8, 0xc7, 0xa2, 0xd4, 0xe4, 0x12, 0x3c, 0x9c, 0xfd, 0xe6, 0x89, 0x66, 0xb9, 0x28,
	0x35, 0x75, 0x7a, 0xb5, 0xd0, 0x1b, 0x74, 0x77, 0x66, 0xb7, 0x8b, 0x04, 0x97, 0x3b, 0x04, 0xd6,
	0x9b, 0x25, 0x38, 0x3f, 0x24, 0x68, 0x04, 0xdf, 0xe0, 0xad, 0x81, 0x56, 0xb3, 0x5c, 0x24, 0x13,
	0x85, 0x05, 0x57, 0x7a, 0x23, 0xb7, 0x67, 0xe6, 0x9f, 0xbd, 0xdd, 0xff, 0xef, 0xbd, 0x80, 0xf6,
	0xe6, 0x9e, 0xd7, 0x55, 0x9e, 0x73, 0x7d, 0x70, 0x8f, 0x99, 0x2d, 0x33, 0xbd, 0x2e, 0xf8, 0xf3,
	0x3d, 0x2e, 0xa0, 0xae, 0x0b, 0x2c, 0x69, 0xcd, 0x12, 0xb6, 0x2d, 0xe1, 0x4b, 0x22, 0x2e, 0xa0,
	0x5e, 0x4e, 0xb0, 0xa4, 0xf5, 0xc3, 0x5e, 0x70, 0x0f, 0x30, 0x4c, 0x96, 0xb7, 0xa2, 0x5c, 0xfc,
	0x2c, 0x34, 0xf1, 0xe1, 0x78, 0x2e, 0xca, 0x05, 0xdb, 0x6a, 0x75, 0xa0, 0x89, 0xc5, 0x9e, 0x90,
	0x0f, 0xc7, 0x45, 0xac, 0x2d, 0xa4, 0x66, 0x3f, 0x66, 0x17, 0xda, 0x66, 0xe6, 0x06, 0xe5, 0x5c,
	0x64, 0xa3, 0x94, 0xd6, 0x6d, 0x48, 0xde, 0xdb, 0x40, 0x45, 0x26, 0x47, 0x84, 0x00, 0xd8, 0x40,
	0xb1, 0x64, 0x9b, 0xa8, 0xe0, 0x1a, 0x4e, 0x36, 0x0e, 0xbf, 0x0a, 0x6d, 0xd4, 0xf7, 0x2d, 0xee,
	0xec, 0xe1, 0xbe, 0x78, 0x56, 0xfc, 0x4f, 0xac, 0x52, 0x66, 0x63, 0x64, 0x95, 0xaf, 0x3e, 0xc1,
	0x69, 0x74, 0x17, 0xaf, 0x78, 0xf4, 0xc8, 0xa5, 0x36, 0x69, 0x25, 0x14, 0xba, 0xe6, 0x97, 0xfd,
	0x40, 0xcd, 0x22, 0x89, 0x55, 0xb6, 0x60, 0x77, 0x98, 0xa7, 0x9d, 0x37, 0x84, 0xc0, 0xa9, 0xed,
	0xdc, 0x56, 0x79, 0xce, 0x4c, 0x40, 0x3b, 0xce, 0xdf, 0x00, 0x00, 0x00, 0xff, 0xff, 0xea, 0xa5,
	0x45, 0x57, 0x4b, 0x03, 0x00, 0x00,
}
