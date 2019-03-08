package internal

import (
	"server/model"
	"server/vector"

	"github.com/name5566/leaf/gate"
)

var PlayerMap = map[int32]*Player{}

type Player struct {
	Unit  *model.Unit
	Agent gate.Agent

	Pos *vector.Vector3
	Dir int32
}

func (p *Player) Init(_pos *vector.Vector3) {
	p.Pos = _pos
}
