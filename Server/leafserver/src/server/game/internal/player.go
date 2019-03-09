package internal

import (
	"github.com/name5566/leaf/gate"
	"server/model"
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
