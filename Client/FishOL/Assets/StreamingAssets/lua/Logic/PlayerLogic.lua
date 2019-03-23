local PlayerLogic = class("PlayerLogic")

function PlayerLogic:Init(data)
	self.info = data
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


return PlayerLogic