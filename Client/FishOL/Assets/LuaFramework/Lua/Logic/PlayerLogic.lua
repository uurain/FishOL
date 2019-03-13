local PlayerLogic = class("PlayerLogic")

function PlayerLogic:Init(data)
	self.info = data
end

function PlayerLogic:CanFire(bulletType)
	local cast = bulletType
	if self.info.player_info.gold < cast then
		return 1
	end
	return 0
end

function PlayerLogic:GetName()
	return self.info.player_info.name
end

function PlayerLogic:IsMine()
	return PlayerData.uid == self.info.uid
end

function PlayerLogic:GetTableIndex()
	return self.info.table_index
end

function PlayerLogic:ReqHitFish(fishId)
	Msg_ReqHitFish(self.info.uid, fishId)
end

return PlayerLogic