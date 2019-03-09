package internal

import (
	"encoding/json"
	"io/ioutil"
	"math/rand"

	"fmt"
	"server/vector"

	"github.com/name5566/leaf/log"
)

type PathData struct {
	Ident   int
	PosList []*vector.Vector3
}

type PathMgr struct {
	pathList []*PathData
}

var pathMgr = &PathMgr{}
var pathCount = 0

func InitPathMgr() {
	for i := 0; i < 1; i++ {
		data, err := ioutil.ReadFile(fmt.Sprintf("conf/path%d.json", i+1))
		if err != nil {
			log.Fatal("%v", err)
		}
		pData := &PathData{}
		err = json.Unmarshal(data, pData)
		if err != nil {
			log.Fatal("%v", err)
			continue
		}
		pathMgr.pathList = append(pathMgr.pathList, pData)
	}
	pathCount = len(pathMgr.pathList)
}

func GetPath() *PathData {
	var randIndex = rand.Intn(pathCount - 1)
	return pathMgr.pathList[randIndex]
}
