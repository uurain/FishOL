local BulletCompt = class(require("Logic.Component.BaseCompt"))

function BulletCompt:ctor()
	self.assetNode = nil
end

function BulletCompt:Init(bulletDb)
	self.super.Init(self)

    self.assetNode = require("Logic.Component.GameObjectNode").new(bulletDb.path)
    self.assetNode:Load(function (assetObj )
        assetObj.CachedTransform:SetParent(self.rootWorldObj.transform)
    end)
end

function BulletCompt:Dispose()
	if self.assetNode ~= nil then
		self.assetNode:Dipose()
		self.assetNode = nil
	end
	self.super.Dipose(self)
end

return BulletCompt