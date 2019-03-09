package internal

import (
	//	"fmt"
	"reflect"
	"server/msg"

	"github.com/name5566/leaf/util"

	"github.com/golang/protobuf/proto"
	"github.com/name5566/leaf/gate"
	"github.com/name5566/leaf/log"
)

func handleMsg(m interface{}, h interface{}) {
	skeleton.RegisterChanRPC(reflect.TypeOf(m), h)
}

func init() {
	handleMsg(&msg.ReqLogin{}, handleLogin)
}

func handleLogin(args []interface{}) {
	// 收到的 Hello 消息
	m := args[0].(*msg.ReqLogin)
	// 消息的发送者
	a := args[1].(gate.Agent)

	uid := util.RandInterval(1000, 9999)
	log.Release("handleLogin %d id:%d", m.GetAccountId(), uid)

	a.SetUserData(uid)
	a.WriteMsg(&msg.AckLogin{
		ErrorCode: proto.Int32(0),
		Uid:       proto.Int32(uid),
	})
}
