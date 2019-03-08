package internal

import (
	"server/model"
	"server/msg"
	"server/vector"

	"github.com/golang/protobuf/proto"
)

func CreatePlayerInfo(unit *model.Unit) *msg.PlayerInfo {
	info := &msg.PlayerInfo{
		Atk:       proto.Int32(unit.Atk),
		Def:       proto.Int32(unit.Def),
		Hp:        proto.Int32(unit.Hp),
		MoveSpeed: proto.Int32(unit.MoveSpeed),
		Name:      proto.String(unit.Name),
	}
	return info
}

func CovertPbVector3(v *vector.Vector3) *msg.Vector3 {
	info := &msg.Vector3{
		X: proto.Float32((float32)(v[0])),
		Y: proto.Float32((float32)(v[1])),
		Z: proto.Float32((float32)(v[2])),
	}
	return info
}
