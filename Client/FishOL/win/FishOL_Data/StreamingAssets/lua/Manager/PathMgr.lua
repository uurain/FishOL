local json = require "Common/json"

PathMgr = {}

local _pathTable = {}
function PathMgr.GetPath(pathId)
	local pathVal = _pathTable[pathId]
	if pathVal == nil then
		_pathTable[pathId] = PathMgr.ReadPath(pathId)
	end
	return _pathTable[pathId]
end

function PathMgr.ReadPath(pathId)
	local jsonStr = FileRead(Util.DataPath.."/config/path"..pathId..".json")
	local val = json.decode(jsonStr)
	local posList = val["posList"]
	local posTable = {}
	for i,v in ipairs(posList) do
		local pos = Vector3.New(v.x, v.y, v.z)
		table.insert(posTable, pos)
	end
	return posTable
end