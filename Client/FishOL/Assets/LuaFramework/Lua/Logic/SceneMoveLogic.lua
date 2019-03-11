local SceneMoveLogic = Class("FishMoveLogic")

function SceneMoveLogic:Init()
	self.fishTable = {}
	self.bulletTable = {}
end

function SceneMoveLogic:CreateFish(fishId, fishDbId, pathId)
	local tempTable = {}
	tempTable.fishId = fishId
	tempTable.fishDbId = fishDbId
	tempTable.pathId = pathId
	local fishDb = nil
	tempTable.compt = require("Logic.Component.FishCompt").new(fishDb)

	self.fishTable[fishId] = tempTable
end

function SceneMoveLogic:RemoveFish(fishId)
	self:RemoveObj(self.fishTable, fishId)
end

function SceneMoveLogic:RemoveBullet(bulletId)
	self:RemoveObj(self.bulletTable, bulletId)
end

function SceneMoveLogic:RemoveObj(objTable, objId)
	local obj = objTable[objId]
	if obj ~= nil then
		obj.compt:Dispose()
	end
	objTable[objId] = nil
end

function SceneMoveLogic:OnUpdate()
	
end

return SceneMoveLogic