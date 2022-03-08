package GoPool

import "github.com/panjf2000/ants"

func InitPool(poolSize int, task func(interface{})) (*ants.PoolWithFunc, error) {
	p, _ := ants.NewPoolWithFunc(poolSize, task)

	return p, nil
}
