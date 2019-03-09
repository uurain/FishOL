package model

import (
	"fmt"
)

type Unit struct {
	Gold          int32
	Name         string
}

func (self *Unit) DebugName() {
	fmt.Println(self.Name)
}
