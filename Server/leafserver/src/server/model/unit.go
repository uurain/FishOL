package model

import (
	"fmt"
)

type Unit struct {
	Uid          int32
	Atk          int32
	Def          int32
	Hp           int32
	HpMax        int32
	MoveSpeed    int32
	Level        int32
	ModelClothes int32
	Name         string
}

func (u *Unit) DebugName() {
	fmt.Println(u.Name)
}
