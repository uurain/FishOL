local RoomLogic = class(require("Logic.BaseLogic"))

function RoomLogic:Init()
	self.playerTable = {}

	Event.AddListener(Msg.Id.AckPlayerEntryList, handler(self, self.OnPlayerEntryList))
	Event.AddListener(Msg.Id.AckPlayerLeaveList, handler(self, self.OnPlayerLeaveList))
	Event.AddListener(Msg.Id.ReqAckBullet, handler(self, self.OnBullet))
	Event.AddListener(Msg.Id.ReqAckLeaveRoom, handler(self, self.OnLeaveRoom))
	Event.AddListener(Msg.Id.AckFishOpt, handler(self, self.OnFishOpt))
	Event.AddListener(Msg.Id.ReqAckHitFish, handler(self, self.OnHitFish))
end

function RoomLogic:OnBullet(msg)
	self.super.DoEvent(self, "Action_Room_Bullet", msg)
end

function RoomLogic:OnLeaveRoom(msg)
	self.super.DoEvent(self, "Action_Room_Leave")
end

function RoomLogic:OnFishOpt(msg)

end

function BoomLogic:OnHitFish(msg)
	self.super.DoEvent(self, "Action_Room_HitFish", msg)
end

function RoomLogic:OnPlayerEntryList(msg)
	for i,v in ipairs(msg.object_list) do
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

	self.super.DoEvent(self, "Action_Room_PlayerEnterLeave", player.table_index, playerInfo)
end

function BoomLogic:ReqFire(bulletType, sPos, tPos)
	Msg_ReqBullet(PlayerData.uid, bulletType, sPos, tPos)
end

function BoomLogic:ReqLeaveRoom()
	Msg_ReqLeaveRoom()
end

return RoomLogic