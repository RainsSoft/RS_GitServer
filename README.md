# RS_GitServer
RS Git Server是一个简易便携版Git服务器，用于快速构建git作为中转的更新服务
# 特点
一个“ git 的完全重新实现”! * *
 
## Features implemented

 * commits
 * branches 
   * (create, list, delete)
   * detached heads
   * HEAD branch
 * checkout branches or commits
 * logging
 * push + pull
 * clone
 * remote (create, list, delete)
 * command line parser
 * store git state on disk 


## Planned work 
	
 * git log 
   * graphical (--graph)
   * patch (-p)
   * "less" implementation
 * git INDEX rather than scanning files
 * blob compression
 * diff'er to show changes to files 
   * and select changes to index...