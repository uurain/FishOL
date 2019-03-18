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

	fishBaseId  int32
}

func (self *Room) Init() {
	self.fishBaseId = 0
	self.createIndex = 0
	self.fishMap = make(map[int32]*Fish)
	go self.BeginTick()
}

func (self *Room) BeginTick() {
	timeTick := time.NewTicker(5 * time.Second)
	for {
		select {
		case <-timeTick.C:
			self.Update()
		}
	}
}

func (self *Room) Update() {
	self.CreateFish()

	for _, v := range  self.fishMap{
		if time.Now().Unix() - v.createTime > 30 {
			self.DeleteFish(v.ident)
			break
		}
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

func (self *Room) RemovePlayer(ident int32) {
	for i, v := range self.playerList {
		if v != nil && v.Ident == ident {
			self.playerList[i] = nil
			break
		}
	}
}

func (self *Room) GetPlayer(ident int32) *Player {
	for _, v := range self.playerList {
		if v != nil && v.Ident == ident {
			return v
		}
	}
	return nil
}

func (self *Room) IsFull() bool {
	for i := range self.playerList{
		if self.playerList[i] == nil {
			return false
		}
	}
	return true
}

func (self *Room) CreateFish() {
	self.fishBaseId ++
	dbId := rand.Int31n(7)+1
	pathDbId := rand.Int31n(5)+1
	fish := &Fish{
		ident: self.fishBaseId,
		configId: dbId,
		pathId: pathDbId,
		createTime:time.Now().Unix(),
		gold:dbId,
	}
	fish.Init()
	self.fishMap[fish.ident] = fish

	msgInfo := &msg.AckFishOpt{
		FishId: proto.Int32(fish.ident),
		OptType:proto.Int32(0),
		PathId: proto.Int32(fish.pathId),
		FishConfigId: proto.Int32(fish.configId),
	}
	self.SyncMsg(0, msgInfo)

	//log.Release("create fish:%v", fish.configId)
}

func (self *Room) DeleteFish(fishId int32){
	_, isExit := self.fishMap[fishId]
	if isExit {
		self.ReqDeleteFish(fishId)
		delete(self.fishMap, fishId)
	}
}

// 收到鱼被命中消息
func (self *Room) AckBehitFish(p *Player, ident int32) {
	fish, isExit := self.fishMap[ident]
	if isExit {
		if rand.Uint32() > 50000 {
			self.ReqBehitFish(p, fish, fish.gold)
			p.Unit.Gold += fish.gold
			delete(self.fishMap, ident)
		} else {
			log.Release("%s未命中%d", p.Ident, ident)
		}
	}
}

// 玩家进入房间
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

// 玩家离开房间
func (self *Room) AckLeaveRoom(playerId int32) {
	var player = self.GetPlayer(playerId)
	if player != nil {
		self.RemovePlayer(playerId)
	}

	msgInfo := &msg.AckPlayerLeaveList{
		ObjectList: []int32{int32(playerId)},
	}
	self.SyncMsg(playerId, msgInfo)
}

// 玩家发射子弹
func (self *Room) AckBullet(playerId int32, sPos *vector.Vector3, tPos *vector.Vector3, configId int32) {
	var player = self.GetPlayer(playerId)
	if player != nil {
		self.ReqBullet(player, sPos, tPos, configId)
		//  这里需要读取子弹消耗的金币
		player.Unit.Gold -= configId
		player.SyncProperty()
	}
}

func (self *Room) ReqBullet(p *Player, sPos *vector.Vector3, tPos *vector.Vector3, configId int32) {
	msgInfo := &msg.ReqAckBullet{
		Uid: proto.Int32(p.Ident),
		BulletType: proto.Int32(configId),
		SPos: CovertPbVector2(sPos),
		Tpos: CovertPbVector2(tPos),
	}
	self.SyncMsg(p.Ident, msgInfo)
}

func (self *Room) ReqBehitFish(p *Player, fish *Fish, addGold int32) {
	msgInfo := &msg.ReqAckHitFish{
		Uid: proto.Int32(p.Ident),
		FishId: proto.Int32(fish.ident),
		RewardGold: proto.Int32(addGold),
	}
	self.SyncMsg(0, msgInfo)
}

func (self *Room) ReqDeleteFish(fishId int32){
	msgInfo := &msg.AckFishOpt{
		FishId:  proto.Int32(fishId),
		OptType: proto.Int32(1),
	}
	self.SyncMsg(0, msgInfo)
}

func (self *Room) SyncMsg(ignoreIdent int32, msg interface{}) {
	for _, p := range self.playerList {
		if p != nil && ignoreIdent != p.Ident {
			p.Agent.WriteMsg(msg)
		}
	}
}
