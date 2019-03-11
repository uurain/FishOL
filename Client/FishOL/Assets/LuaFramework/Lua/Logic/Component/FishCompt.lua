local FishCompt = class(require("Logic.Component.BaseCompt"))

function FishCompt:ctor()
	self.assetNode = nil
end

function FishCompt:Init(fishDb)
	self.super.Init(self)

    self.assetNode = require("Logic.Component.GameObjectNode").new(fishDb.path)
    self.assetNode:Load(function (assetObj )
        assetObj.CachedTransform:SetParent(self.rootWorldObj.transform)
    end)
end

function FishCompt:Dispose()
	if self.assetNode ~= nil then
		self.assetNode:Dipose()
		self.assetNode = nil
	end
	self.super.Dipose(self)
end

return FishCompt