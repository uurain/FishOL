package gate

import (
	"server/game"
	"server/login"
	"server/msg"
)

func init() {
	msg.Processor.SetRouter(&msg.ReqLogin{}, login.ChanRPC)
	msg.Processor.SetRouter(&msg.ReqAckPlayerMove{}, game.ChanRPC)
	msg.Processor.SetRouter(&msg.ReqAckStop{}, game.ChanRPC)
}
