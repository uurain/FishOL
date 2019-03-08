package internal

import (
	"server/model"
	"server/msg"
	"server/vector"

	"github.com/golang/protobuf/proto"

	"github.com/name5566/leaf/gate"
)

func init() {
	skeleton.RegisterChanRPC("NewPlayer", rpcNewAgent)
	skeleton.RegisterChanRPC("ClosePlayer", rpcCloseAgent)

	WorldScene.Init()
}

func rpcNewAgent(args []interface{}) {
	a := args[0].(gate.Agent)
	//	_ = a
	unit := args[1].(model.Unit)

	initPos := vector.NewVector3(0, 0, 0)
	var initDir int32 = 0

	p := &Player{
		Unit:  &unit,
		Agent: a,
		Pos:   initPos,
		Dir:   initDir,
	}
	PlayerMap[unit.Uid] = p

	a.WriteMsg(&msg.AckPlayerEntryList{
		ObjectList: []*msg.PlayerEntryInfo{
			&msg.PlayerEntryInfo{
				ObjectGuid: proto.Int32(unit.Uid),
				X:          proto.Float32((float32)(initPos[0])),
				Y:          proto.Float32((float32)(initPos[1])),
				Z:          proto.Float32((float32)(initPos[2])),
				R:          proto.Int32(initDir),
				PlayerInfo: CreatePlayerInfo(&unit),
			},
		},
	})
	WorldScene.AddPlayer(p)
}

func rpcCloseAgent(args []interface{}) {
	a := args[0].(gate.Agent)
	_ = a
	uid := args[1].(int32)
	p, ok := PlayerMap[uid]
	if ok {
		WorldScene.RemovePlayer(uid)
		p.Agent.Close()
		delete(PlayerMap, uid)
	}
}
