package gate

import (
	"server/game"
	"server/login"
	"server/msg"
)

func init() {
	msg.Processor.SetRouter(&msg.ReqLogin{}, login.ChanRPC)
	
	msg.Processor.SetRouter(&msg.ReqAckEnterRoom{}, game.ChanRPC)
	msg.Processor.SetRouter(&msg.ReqAckLeaveRoom{}, game.ChanRPC)
	msg.Processor.SetRouter(&msg.ReqAckBullet{}, game.ChanRPC)
	msg.Processor.SetRouter(&msg.ReqAckHitFish{}, game.ChanRPC)
}
