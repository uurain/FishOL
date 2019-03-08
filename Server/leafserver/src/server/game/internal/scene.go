package internal

import (
	"fmt"
	"server/msg"
	"time"

	"github.com/golang/protobuf/proto"
)

var WorldScene *Scene = &Scene{}

type Scene struct {
	playMap map[int32]*Player

	updateTimer *time.Ticker
	Id          int32
}

func (s *Scene) Init() {
	fmt.Println("Scene init")
	//s.Id = 101
	//s.updateTimer = time.NewTicker(50 * time.Millisecond)
	//for {
	//	select {
	//	case <-s.updateTimer.C:
	//		s.Update()
	//	}
	//}
}

func (s *Scene) Destroy() {
	if s.updateTimer != nil {
		s.updateTimer.Stop()
		s.updateTimer = nil
	}
}

// 1秒20帧的频率执行
func (s *Scene) Update() {
		fmt.Println("sceneUpdate")
}

func (s *Scene) AddPlayer(p *Player) {
	p.Agent.WriteMsg(&msg.AckSwapScene{
		SceneId: proto.Int32(s.Id),
		X:       proto.Float32((float32)(p.Pos[0])),
		Y:       proto.Float32((float32)(p.Pos[1])),
		Z:       proto.Float32((float32)(p.Pos[2])),
		R:       proto.Int32(p.Dir),
	})

	for k, v := range s.playMap {
		_ = k
		pos := p.Pos
		v.Agent.WriteMsg(&msg.AckPlayerEntryList{
			ObjectList: []*msg.PlayerEntryInfo{
				&msg.PlayerEntryInfo{
					ObjectGuid: proto.Int32(p.Unit.Uid),
					X:          proto.Float32((float32)(pos[0])),
					Y:          proto.Float32((float32)(pos[1])),
					Z:          proto.Float32((float32)(pos[2])),
					R:          proto.Int32(0),
					PlayerInfo: CreatePlayerInfo(p.Unit),
				},
			},
		})
	}
	s.playMap[p.Unit.Uid] = p
}

func (s *Scene) RemovePlayer(uid int32) {
	s.AckMsg(uid, &msg.AckPlayerLeaveList{
		ObjectList: []int32{
			uid,
		},
	})
	delete(s.playMap, uid)
}

func (s *Scene) MovePlayer(uid int32, targetPos *msg.Vector3) {
	s.AckMsg(0, &msg.ReqAckPlayerMove{
		Mover:     proto.Int32(uid),
		TargetPos: targetPos,
	})
}

func (s *Scene) StopMove(stopMsg *msg.ReqAckStop) {
	s.AckMsg(0, stopMsg)
}

func (s *Scene) AckMsg(ingoreUid int32, msg interface{}) {
	for k, v := range s.playMap {
		if ingoreUid == 0 || ingoreUid != k {
			v.Agent.WriteMsg(msg)
		}
	}
}

//同步场景中所有的玩家
func (s *Scene) SyncScenePlayer() {

}
