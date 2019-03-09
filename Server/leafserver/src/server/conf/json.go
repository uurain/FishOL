package conf

import (
	"encoding/json"
	"io/ioutil"

	"github.com/name5566/leaf/log"
)

var Server struct {
	LogLevel    string
	LogPath     string
	WSAddr      string
	CertFile    string
	KeyFile     string
	TCPAddr     string
	MaxConnNum  int
	ConsolePort int
	ProfilePath string
}

func init() {
	//var str1 = "123132"
	//data2 := []byte(str1)
	//ioutil.WriteFile("./yujujun.txt", data2, 9044)

	data, err := ioutil.ReadFile("./conf/server.json")
	if err != nil {
		log.Fatal("test%v", err)
	}
	err = json.Unmarshal(data, &Server)
	if err != nil {
		log.Fatal("%v", err)
	}
}
