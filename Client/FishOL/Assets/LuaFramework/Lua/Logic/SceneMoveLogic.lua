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
	
	self.updateHandle = UpdateBeat:CreateListener(self.OnUpdate, self)
	UpdateBeat:AddListener(self.updateHandle)	
end

function SceneMoveLogic:CreateFish(fishId, fishDbId, pathId)
	log("CreateFish:"..fishDbId)
	local tempTable = self:GetCached(fishDbId, _EType.fish)
	local fromCached = true
	if tempTable == nil then
		tempTable = {}
		fromCached = false
	end
	tempTable.id = fishId
	tempTable.dbId = fishDbId
	tempTable.pathId = pathId
	if not fromCached then
		local fishDb = DbMgr.GetFishDb(fishDbId)
		tempTable.db = fishDb
		tempTable.compt = require("Logic.Component.FishCompt").new()
		tempTable.compt:Init(fishDb)
	end
	tempTable.compt:Begin(pathId, fishId)
	self.fishTable[fishId] = tempTable
end

function SceneMoveLogic:CreateBullet(bulletDbId, sPos, tPos)
	local tempTable = self:GetCached(bulletDbId, _EType.bullet)
	local fromCached = true
	if tempTable == nil then
		tempTable = {}
		fromCached = false
	end
	tempTable.dbId = bulletDbId
	tempTable.tPos = tPos
	if not fromCached then
		local bulletDb = DbMgr.GetBulletDb(bulletDbId)
		tempTable.db = bulletDb
		tempTable.compt = require("Logic.Component.BulletCompt").new(bulletDb)
		tempTable.compt:Init(bulletDb)
	end
	tempTable.compt:Begin(sPos, tPos)
	table.insert(self.bulletTable, tempTable)
end

function SceneMoveLogic:OnFishTrigger(fishId)
	local fishTable = self.fishTable[fishId]
	if fishTable ~= nil then
		fishTable.compt:PlayAni("heart")
	end
end

function SceneMoveLogic:RemoveFish(id)
	local obj = self.fishTable[id]
	if obj ~= nil then
		self:AddCached(obj.dbId, _EType.fish, obj)
	end
	self.fishTable[id] = nil
end

function SceneMoveLogic:RemoveBullet(obj)
	for k,v in pairs(self.bulletTable) do
		if v == obj then		
			self:AddCached(obj.dbId, _EType.bullet, obj)
			self.bulletTable[k] = nil
			break
		end
	end
end

function SceneMoveLogic:GetCached(dbId, etype)
	local cachedDbIdTable = self.cachedTable[etype][dbId]
	if cachedDbIdTable ~= nil then
		local count = #cachedDbIdTable
		if count > 0 then
			local obj = cachedDbIdTable[count]
			cachedDbIdTable[count] = nil
			return obj
		end
	end
	return cachedObj
end

function SceneMoveLogic:AddCached(dbId, etype, obj)
	local cachedDbIdTable = self.cachedTable[etype][dbId]
	if cachedDbIdTable == nil then
		cachedDbIdTable = {}
		self.cachedTable[etype][dbId] = cachedDbIdTable
	end
	table.insert(cachedDbIdTable, obj)
end

function SceneMoveLogic:Dispose()
	UpdateBeat:RemoveListener(self.updateHandle)
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
		if v.compt.isEnd then
			v.compt.isEnd = false
			self:RemoveBullet(v)
		else
			v.compt:OnUpdate(Time.deltaTime)
		end		
	end

	for k, v in pairs(self.fishTable) do
		v.compt:OnUpdate(Time.deltaTime)
	end
end

return SceneMoveLogic