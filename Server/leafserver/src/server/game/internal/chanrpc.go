package internal

import (
	"github.com/name5566/leaf/gate"
	"github.com/name5566/leaf/log"
	"server/model"
)

var playerMap = make(map[int32]*Player)

func init() {
	skeleton.RegisterChanRPC("NewAgent", rpcNewAgent)
	skeleton.RegisterChanRPC("CloseAgent", rpcCloseAgent)

	skeleton.RegisterChanRPC("NewPlayer", rpcNewPlayer)
}

func rpcNewAgent(args []interface{}) {
	_= args
}

func rpcNewPlayer(args []interface{}){
	unit := args[0].(*model.Unit)
	uid := args[1].(int32)
	agent := args[2].(gate.Agent)

	var oldPlayer, ok = playerMap[uid]
	if ok {
		oldPlayer.Agent.Close()
		delete(playerMap, uid)
	}
	player := &Player{
		Ident :uid,
		Unit: unit,
		Agent: agent,
	}
	playerMap[uid] = player
	player.SyncProperty()
}

func rpcCloseAgent(args []interface{}) {
	a := args[0].(gate.Agent)
	switch a.UserData().(type) {
	case int32:
		var uid = a.UserData().(int32)
		log.Release("rpcCloseAgent uid:%v", uid)
		RmovePlayer(uid)
		room.AckLeaveRoom(uid)
		break
	}
}

func RmovePlayer(uid int32)  {
	var _, ok = playerMap[uid]
	if ok {
		delete(playerMap, uid)
	}
}

func GetPlayer(uid int32) * Player  {
	var player, ok = playerMap[uid]
	if ok {
		return player
	}
	return nil
}
