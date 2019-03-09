package internal

import (
	"server/msg"
	"server/vector"

	"github.com/golang/protobuf/proto"
)


func CovertPbVector2(v *vector.Vector3) *msg.Vector2 {
	info := &msg.Vector2{
		X: proto.Float32((float32)(v[0])),
		Y: proto.Float32((float32)(v[2])),
	}
	return info
}

func ConvertVector2To3(v *msg.Vector2)*vector.Vector3{
	val := &vector.Vector3{}
	val.Set(float64(v.GetX()), 0, float64(v.GetY()))
	return val
}
