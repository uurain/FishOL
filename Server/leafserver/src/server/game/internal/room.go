package internal

import (
	"github.com/golang/protobuf/proto"
	"math/rand"
	"server/msg"
	"server/vector"
	"time"

	"github.com/name5566/leaf/log"
)

type Room struct {
	ident       int32
	playerList  [4]*Player
	fishMap     map[int32]*Fish
	createIndex int
}

func (self *Room) Init() {
	self.createIndex = 0
	self.fishMap = make(map[int32]*Fish)
	go self.BeginTick()
}

func (self *Room) BeginTick() {
	timeTick := time.NewTicker(1 * time.Second)
	for {
		select {
		case <-timeTick.C:
			self.Update()
		}
	}
}

func (self *Room) Update() {
	self.createIndex++
	if self.createIndex >= 5 {
		self.createIndex = 0
		self.CreateFish()
	}
}

func (self *Room) AddPlayer(p *Player) {
	for i, v := range self.playerList {
		if v == nil {
			self.playerList[i] = p
			break
		}
	}
}

func (self *Room) RemovePlayer(ident int) {
	for i, v := range self.playerList {
		if v != nil && v.Ident == ident {
			self.playerList[i] = nil
			break
		}
	}
}

func (self *Room) GetPlayer(ident int) *Player {
	for _, v := range self.playerList {
		if v != nil && v.Ident == ident {
			return v
		}
	}
	return nil
}

func (self *Room) CreateFish() {
	fish := &Fish{}
	fish.Init()
	self.fishMap[fish.ident] = fish

	msgInfo := &msg.AckFishOpt{
		FishId: proto.Int32(fish.ident),
		OptType:proto.Int32(0),
		PathId: proto.Int32(0),
		FishConfigId: proto.Int32(1001),
	}
	self.SyncMsg(0, msgInfo)
}

// 收到鱼被命中消息
func (self *Room) AckBehitFish(p *Player, ident int32) {
	fish, isExit := self.fishMap[ident]
	if isExit {
		if rand.Uint32() > 50000 {
			self.ReqBehitFish(p, fish)
			delete(self.fishMap, ident)
		} else {
			log.Release("%s未命中%d", p.Ident, ident)
		}
	}
}

func (self *Room) AckEnterRoom(player *Player){
	self.AddPlayer(player)

	var objectList []*msg.PlayerEntryInfo
	for i, v := range self.playerList{
		if v != nil {
			objectList = append(objectList, &msg.PlayerEntryInfo{
				Uid: proto.Int32(int32(v.Ident)),
				TableIndex: proto.Int32(int32(i)),
				PlayerInfo: &msg.PlayerInfo{
					Gold: proto.Int32(v.Unit.Gold),
					Name: proto.String(v.Unit.Name),
				},
			})
		}
	}
	msgInfo := &msg.AckPlayerEntryList{
		ObjectList : objectList,
	}
	self.SyncMsg(0, msgInfo)
}

func (self *Room) AckLeaveRoom(playerId int) {
	var player = self.GetPlayer(playerId)
	if player != nil {
		self.RemovePlayer(playerId)
	}

	msgInfo := &msg.AckPlayerLeaveList{
		ObjectList: []int32{int32(playerId)},
	}
	self.SyncMsg(playerId, msgInfo)
}

func (self *Room) AckBullet(playerId int, sPos *vector.Vector3, tPos *vector.Vector3, configId int) {
	var player = self.GetPlayer(playerId)
	if player != nil {
		self.ReqBullet(player, sPos, tPos, configId)
	}
}

func (self *Room) ReqBullet(p *Player, sPos *vector.Vector3, tPos *vector.Vector3, configId int) {
	msgInfo := &msg.ReqAckBullet{
		Uid: proto.Int32(int32(p.Ident)),
		BulletType: proto.Int32(int32(configId)),
		SPos: CovertPbVector2(sPos),
		Tpos: CovertPbVector2(tPos),
	}
	self.SyncMsg(p.Ident, msgInfo)
}

func (self *Room) ReqBehitFish(p *Player, fish *Fish) {
	msgInfo := &msg.AckFishOpt{
		FishId: proto.Int32(fish.ident),
		OptType:proto.Int32(1),
	}
	self.SyncMsg(0, msgInfo)
}

func (self *Room) SyncMsg(ignoreIdent int, msg interface{}) {
	for _, p := range self.playerList {
		if ignoreIdent != p.Ident {
			p.Agent.WriteMsg(msg)
		}
	}
}
