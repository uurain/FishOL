package internal

import (
	//	"fmt"
	"reflect"
	"server/game"
	"server/model"
	"server/msg"

	"github.com/name5566/leaf/util"

	"github.com/golang/protobuf/proto"
	"github.com/name5566/leaf/gate"
	"github.com/name5566/leaf/log"
)

var uidMap = map[string]int32{}

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

	localUid, ok := uidMap[m.GetAccountId()]
	if !ok {
		// 已经登陆过了 把之前的玩家踢下线
		game.ChanRPC.Go("ClosePlayer", localUid)
	}

	//	fmt.Println(m.GetUseId())
	// 输出收到的消息的内容
	randId := util.RandInterval(1000, 9999)
	uidMap[m.GetAccountId()] = randId
	log.Debug("handleLogin %d id:%d", m.GetAccountId(), randId)

	unit := model.Unit{
		Uid:       randId,
		Atk:       10,
		Def:       1,
		Hp:        1000,
		MoveSpeed: 5,
		Name:      m.GetAccountId(),
	}
	game.ChanRPC.Go("NewPlayer", unit)

	a.WriteMsg(&msg.AckLogin{
		ErrorCode: proto.Int32(0),
		Uid:       proto.Int32(randId),
	})
}
