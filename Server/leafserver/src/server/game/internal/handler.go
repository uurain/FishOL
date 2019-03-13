package internal

import (
	"github.com/golang/protobuf/proto"
	"github.com/name5566/leaf/gate"
	"reflect"
	"server/msg"
)

func handleMsg(m interface{}, h interface{}) {
	skeleton.RegisterChanRPC(reflect.TypeOf(m), h)
}

var room = &Room{}

func init() {
	handleMsg(&msg.ReqAckEnterRoom{}, handleEnterRoom)
	handleMsg(&msg.ReqAckLeaveRoom{}, handleLeaveRoom)
	handleMsg(&msg.ReqAckBullet{}, handleBullet)

	room.Init()
}

func handleEnterRoom(args []interface{}) {
	agent := args[1].(gate.Agent)
	uid := agent.UserData().(int32)
	player := GetPlayer(uid)
	if player != nil {
		if room.IsFull() {
			agent.WriteMsg(&msg.AckError{
				ErrorCode: proto.Int32(int32( msg.EGameEventCode_Code_Full_Room)),
			})
		}else{
			agent.WriteMsg(&msg.ReqAckEnterRoom{})
			room.AckEnterRoom(player)
		}
	}
}

func handleLeaveRoom(args []interface{}) {
	agent := args[1].(gate.Agent)
	uid := agent.UserData().(int32)
	room.AckLeaveRoom(uid)
}

func handleBullet(args []interface{}) {
	msgInfo := args[0].(*msg.ReqAckBullet)
	agent := args[1].(gate.Agent)
	uid := agent.UserData().(int32)
	player := GetPlayer(uid)
	if player != nil {
		if player.CanFire(msgInfo.GetBulletType()) {
			room.AckBullet(uid, ConvertVector2To3(msgInfo.GetSPos()), ConvertVector2To3(msgInfo.GetTpos()), int(msgInfo.GetBulletType()))
		}else
		{
			agent.WriteMsg(&msg.AckError{
				ErrorCode: proto.Int32(int32(msg.EGameEventCode_Code_Not_Enough_Gold)),
			})
		}
	}
}
