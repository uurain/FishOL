local UITip = class(require("View.BasePanel"))

function UITip:Init()
	self.super:Init()

	self.tipItemTable = {}
end

function UITip:ShowTip(txt)
	local tipItem = UIPackage.CreateObject("Common", "tipItem")
	self.view:AddChild(tipItem)
	tipItem:Center()
	tipItem:GetTransition("t0"):Play()
	tipItem.title = txt

	local count = #self.tipItemTable
	for i,v in ipairs(self.tipItemTable) do
		local offset = (count + 1 - i) * 55 
		v.y = tipItem.y - offset
	end

	if count > 10 then
		local item = self.tipItemTable[1]
		item:Dispose()
		table.remove(self.tipItemTable, 1)
	end
	table.insert(self.tipItemTable, tipItem)
end


return UITip