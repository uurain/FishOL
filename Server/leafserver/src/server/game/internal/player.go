package internal

import (
	"github.com/golang/protobuf/proto"
	"github.com/name5566/leaf/gate"
	"server/model"
	"server/msg"
)

type Player struct {
	Ident int32
	Unit *model.Unit
	Agent gate.Agent
}

func (self *Player) Init() {

}

func (self *Player) CanFire(bulletId int32) bool{
	return true
}

func (self *Player) SyncProperty() {
	self.Agent.WriteMsg(&msg.AckPublicPropertyList{
		Uid: proto.Int32(self.Ident),
		PlayerInfo: &msg.PlayerInfo{
			Gold: proto.Int32(self.Unit.Gold),
			Name: proto.String(self.Unit.Name),
		},
	})
}
