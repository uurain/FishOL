local RoomLogic = class(require("Logic.BaseLogic"))

function RoomLogic:Init()
	self.playerTable = {}

	Event.AddListener(Msg.Id.AckPlayerEntryList, handler(self.OnPlayerEntryList, self))
	Event.AddListener(Msg.Id.AckPlayerLeaveList, handler(self.OnPlayerLeaveList, self))
	Event.AddListener(Msg.Id.ReqAckBullet, handler(self.OnBullet, self))
	Event.AddListener(Msg.Id.ReqAckLeaveRoom, handler(self.OnLeaveRoom, self))
	Event.AddListener(Msg.Id.AckFishOpt, handler(self.OnFishOpt, self))
	Event.AddListener(Msg.Id.ReqAckHitFish, handler(self.OnHitFish, self))

	self.sceneMoveLogic = LogicManager.Get(LogicType.SceneMove)
end

function RoomLogic:OnBullet(msg)
	self.sceneMoveLogic:CreateBullet(msg.bullet_type, msg.uid, Vector3.New(msg.sPos.x, msg.sPos.y, 20), Vector3.New(msg.tPos.x, msg.tPos.y, 20))

end

function RoomLogic:OnLeaveRoom(msg)
	self.super.DoEvent(self, "Action_Room_Leave")
end

function RoomLogic:OnFishOpt(msg)
	if msg.opt_type == 0 then
		self.sceneMoveLogic:CreateFish(msg.fish_id, msg.fishConfigId, msg.path_id)
	elseif msg.opt_type == 1 then
		self.sceneMoveLogic:RemoveFish(msg.fish_id)
	end
end

function RoomLogic:OnHitFish(msg)	
	self.sceneMoveLogic:HitFishSucess(msg.fish_id)

	if msg.uid == PlayerData.uid then
		self.super.DoEvent(self, "Action_Room_HitFish", msg.reward_gold)
	end
end

function RoomLogic:OnPlayerEntryList(msg)
	for i,v in ipairs(msg.object_list) do
		log("OnPlayerEntryList1")
		self:PlayerEnter(v)
	end
end

function RoomLogic:OnPlayerLeaveList(msg)
	for i,v in ipairs(msg.object_list) do
		self:PlayerLeave(v)
	end
end

function RoomLogic:PlayerLeave(uid)
	for k,v in pairs(self.playerTable) do
		if v.uid == uid then
			self.super.DoEvent(self, "Action_Room_PlayerEnterLeave", v.table_index)
			self.playerTable[k] = nil
			break
		end
	end
end

function RoomLogic:PlayerEnter(playerInfo)
	local player = require("Logic/PlayerLogic").new()
	player:Init(playerInfo)
	self.playerTable[playerInfo.table_index] = player

	if playerInfo.uid == PlayerData.uid then
		PlayerData.gold = playerInfo.player_info.gold
	end
	self.super.DoEvent(self, "Action_Room_PlayerEnterLeave", playerInfo.table_index, player)
end

function RoomLogic:CanFire(bulletId)
	local cast = DbMgr.GetBulletDb(bulletId).cast
	if PlayerData.gold < cast then
		return false
	end
	return true
end

function RoomLogic:ReqFire(bulletDbId, sPos, tPos)
	self.sceneMoveLogic:CreateBullet(bulletDbId, PlayerData.uid, sPos, tPos)
	Msg_ReqBullet(PlayerData.uid, bulletDbId, sPos, tPos)
end

function RoomLogic:ReqLeaveRoom()
	Msg_ReqLeaveRoom()
end

return RoomLogic