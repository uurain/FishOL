local SceneMoveLogic = class("FishMoveLogic")

local _EType = {
	fish = 1, bullet = 2
}

function SceneMoveLogic:Init()
	self.fishTable = {}
	self.bulletTable = {}

	self.cachedTable = {}
	self.cachedTable[_EType.fish] = {}
	self.cachedTable[_EType.bullet] = {}
end

function SceneMoveLogic:CreateFish(fishId, fishDbId, pathId)
	log("CreateFish:"..fishDbId)
	local tempTable = self:GetCached(fishDbId, _EType.fish)
	local fromCached = true
	if tempTable == nil then
		tempTable = {}
		fromCached = false
	end
	tempTable.fishId = fishId
	tempTable.fishDbId = fishDbId
	tempTable.pathId = pathId
	if not fromCached then
		local fishDb = DbMgr.GetFishDb(fishDbId)
		tempTable.db = fishDb
		tempTable.compt = require("Logic.Component.FishCompt").new()
		tempTable.compt:Init(fishDb)
	end
	self.fishTable[fishId] = tempTable
end

function SceneMoveLogic:CreateBullet(bulletId, bulletDbId, sPos, tPos)
	local tempTable = self:GetCached(bulletId, _EType.bullet)
	local fromCached = true
	if tempTable == nil then
		tempTable = {}
		fromCached = false
	end
	tempTable.fishId = fishId
	tempTable.bulletDbId = bulletDbId
	tempTable.tPos = tPos
	if not fromCached then
		local bulletDb = DbMgr.GetBulletDb(bulletDbId)
		tempTable.db = bulletDb
		tempTable.compt = require("Logic.Component.BulletCompt").new(bulletDb)
	end
	tempTable.compt:SetLocalPos(sPos)
	self.bulletTable[fishId] = tempTable
end

function SceneMoveLogic:RemoveFish(fishId)
	self:RemoveObj(self.fishTable, fishId, _EType.fish)
end

function SceneMoveLogic:RemoveBullet(bulletId)
	self:RemoveObj(self.bulletTable, bulletId, _EType.bullet)
end

function SceneMoveLogic:GetCached(dbId, etype)
	local cachedObj = self.cachedTable[etype][dbId]
	if cachedObj ~= nil then
		self.cachedTable[etype][dbId] = nil
	end
	return cachedObj
end

function SceneMoveLogic:RemoveObj(objTable, objId, etype)
	local obj = objTable[objId]
	if obj ~= nil then
		self.cachedTable[etype][obj.fishDbId] = obj
	end
	objTable[objId] = nil
end

function SceneMoveLogic:Dispose()
	self:ClearTable(self.cachedTable[_EType.fish])
	self:ClearTable(self.cachedTable[_EType.bullet])
	self:ClearTable(self.fishTable)
	self:ClearTable(self.bulletTable)
end

function SceneMoveLogic:ClearTable(objTable)
	for k,v in pairs(objTable) do
		v.compt:Dispose()
	end
	objTable = {}
end

function SceneMoveLogic:OnUpdate()
	for k,v in pairs(self.bulletTable) do
		
	end
end

return SceneMoveLogic