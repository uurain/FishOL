DbMgr = {}

function DbMgr.Init()
	for i,v in ipairs(preLoadData) do
		require(v)
	end
end

function DbMgr.GetFishDb(id)
	for i,v in ipairs(fish) do
		if v.id == id then
			return v
		end
	end
	return nil
end

function DbMgr.GetBulletDb(id)
	for i,v in ipairs(bullet) do
		if v.id == id then
			return v
		end
	end
	return nil
end