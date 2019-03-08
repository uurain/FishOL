package internal

import (
	"reflect"
	"server/msg"
)

func handleMsg(m interface{}, h interface{}) {
	skeleton.RegisterChanRPC(reflect.TypeOf(m), h)
}

func init() {
	handleMsg(&msg.ReqAckStop{}, handleStopMove)
	handleMsg(&msg.ReqAckPlayerMove{}, handleMove)
}

func handleStopMove(args []interface{}) {
	m := args[0].(*msg.ReqAckStop)
	WorldScene.StopMove(m)
}

func handleMove(args []interface{}) {
	//WorldScene.MovePlayer(args[0].(*msg.ReqAckPlayerMove).Mover, args[0].(*msg.ReqAckPlayerMove).TargetPos)
}
