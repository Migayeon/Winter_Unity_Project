import json, os

saveJson = {}

class Node:
    def __init__(self, text = None, manageValue = None, description = None, query = None, manageList = None, manageDict = None, isRoot = False) -> None:
        self.text = text
        self.manageValue = manageValue
        self.manageList = manageList
        self.manageDict = manageDict
        self.description = description
        self.query = query
        self.isRoot = isRoot

        self.child = None
        self.sibling = None

    def updateNewVariable(self):
        children = self.getChildren()
        if self.manageList is not None:
            tmp = saveJson
            for i in range(len(self.manageList) - 1):
                tmp = tmp[self.manageList[i]]
            if type(tmp) == dict:
                if tmp.get(self.manageList[-1]) is None:
                    tmp[self.manageList[-1]] = []
            elif type(tmp) == list:
                while len(tmp) < self.manageList[-1]:
                    tmp.append([])
        
        if self.manageDict is not None:
            tmp = saveJson
            for i in range(len(self.manageDict) - 1):
                tmp = tmp[self.manageDict[i]]
            if type(tmp) == dict:
                if tmp.get(self.manageDict[-1]) is None:
                    tmp[self.manageDict[-1]] = {}
            elif type(tmp) == list:
                while len(tmp) < self.manageDict[-1]:
                    tmp.append([])

        if children is None:
            if self.manageValue is not None:
                tmp = saveJson
                for i in range(len(self.manageValue) - 1):
                    tmp = tmp[self.manageValue[i]]
                if type(tmp) == dict:
                    if tmp.get(self.manageValue[-1]) is None:
                        tmp[self.manageValue[-1]] = 0
                elif type(tmp) == list:
                    while len(tmp) < self.manageValue[-1]:
                        tmp.append(0)
        else:
            for child in children:
                child.updateNewVariable()
    
    def getSiblings(self):
        if self.sibling is None:
            return [self]
        return [self] + self.sibling.getSiblings()
    
    def getChildren(self):
        if self.child is not None:
            return self.child.getSiblings()
        elif self.query is not None:
            return self.query().getSiblings()
            
    def __str__(self):
        if self.description is None:
            return self.text
        else:
            return self.text + f" [{self.description}]"

    def __repr__(self):
        if self.description is None:
            return self.text
        else:
            return self.text + f" [{self.description}]"
    
def query(queryValue : list, queryId : str):
    def stat():
        root = Node("강의력", manageValue = [*queryValue, 0])
        root.sibling = Node("마법이론", manageValue = [*queryValue, 1])
        root.sibling.sibling = Node("마나감응", manageValue = [*queryValue, 2])
        root.sibling.sibling.sibling = Node("손재주", manageValue = [*queryValue, 3])
        root.sibling.sibling.sibling.sibling = Node("속성력", manageValue = [*queryValue, 4])
        root.sibling.sibling.sibling.sibling.sibling = Node("영창", manageValue = [*queryValue, 5])
        return root

    def stdProfessor():
        root = Node("총 스탯 포인트", manageValue = [*queryValue, "StatPoint"])
        root.sibling = Node("봉급 조절", manageValue = [*queryValue, "SalarySale"], description = "[총 스탯] / [봉급 조절] 이 봉급이 됨")
        root.sibling.sibling = Node("항목별 기본 제공 스탯", manageList = [*queryValue, "FixedStat"], query = query([*queryValue, "FixedStat"], "setStat"), description = "기본 제공으로 고정된 경우 추가적인 스탯분배는 받지 못합니다.")
        root.sibling.sibling.sibling = Node("항목별 최소 스탯", manageList = [*queryValue, "MinStat"], query = query([*queryValue, "MinStat"], "setStat"))
        return root
    
    queryManager = {
        "setStat" : stat,
        "stdProfessor" : stdProfessor
    }

    return queryManager[queryId]

def save(data, key):
    rst = ""
    if type(data[key]) != dict:
        return data[key]
    if not any(type(i) == dict for i in data[key]):
        rst += json.dumps(data[key], indent = 4)
    else:
        for i in data[key].keys():
            rst += key + ":" + save(data[key], i)
    return rst

root = Node("~", isRoot = True)
manageKey = ["Professor"]
root.child = Node("교수", manageDict = manageKey)
root.child.child = Node("교수 생성")
root.child.child.child = Node("기초 교수")
root.child.child.child.child = Node("일반 교수", manageDict = [*manageKey, "StdNormal"], query = query([*manageKey, "StdNormal"], "stdProfessor"))
root.child.child.child.child.sibling = Node("전투 교수", manageDict = [*manageKey, "StdBattle"], query = query([*manageKey, "StdBattle"], "stdProfessor"))
root.child.child.child.child.sibling.sibling = Node("유니크 교수", manageDict = [*manageKey, "StdUnique"], query = query([*manageKey, "StdUnique"], "stdProfessor"))
root.child.child.child.child.sibling.sibling.sibling = Node("지급 옵션")
root.child.child.child.child.sibling.sibling.sibling.child = Node("일반 교수 수", manageValue = [*manageKey, "StdNormalCnt"])
root.child.child.child.child.sibling.sibling.sibling.child.sibling = Node("전투 교수 수", manageValue = [*manageKey, "StdBattleCnt"])
root.child.child.child.child.sibling.sibling.sibling.child.sibling.sibling = Node("유니크 교수 수", manageValue = [*manageKey, "StdUniqueCnt"])

root.child.child.child.sibling = Node("임용 교수")
root.child.child.child.sibling.child = Node("일반 교수", manageDict = [*manageKey, "Normal"], query = query([*manageKey, "Normal"], "stdProfessor"))
root.child.child.child.sibling.child.sibling = Node("전투 교수", manageDict = [*manageKey, "Battle"], query = query([*manageKey, "Battle"], "stdProfessor"))
root.child.child.child.sibling.child.sibling.sibling = Node("유니크 교수", manageDict = [*manageKey, "Unique"], query = query([*manageKey, "Unique"], "stdProfessor"))
root.child.child.child.sibling.child.sibling.sibling.sibling = Node("등장 확률", description = "전투 교수와 유니크 교수 외의 교수는 모두 일반 교수가 됩니다")
root.child.child.child.sibling.child.sibling.sibling.sibling.child = Node("전투 교수 등장 확률", manageValue = [*manageKey, "BattlePercent"])
root.child.child.child.sibling.child.sibling.sibling.sibling.child.sibling = Node("유니크 교수 등장 확률", manageValue = [*manageKey, "UniquePercent"])

if __name__ == "__main__":
    print("< 밸런스 컨트롤러 >")
    addr = root
    addrSave = []
    while True:
        print("현재 주소 : ", '/'.join(map(lambda x : x.text, addrSave + [addr])))
        flag = addr.manageValue is None
        if flag:
            if addr.child is not None:
                options = addr.getChildren()
            elif addr.query is not None:
                options = addr.query().getSiblings()
            idx = 0
            for option in options:
                print(f'{idx} : {option}')
                idx += 1
            print("s : 저장하기")
            if not addr.isRoot:
                print("b : 뒤로가기")
            else:
                print("i : 변수 갱신하기")
            print("q : 종료하기")
            print("c : 지우기")
            selection = input(">>> ")
            if selection.isdigit():
                selection = int(selection)
                addrSave.append(addr)
                addr = options[selection]
            elif selection == "s":
                rst = {}
                for i in saveJson.keys():
                    rst[i] = save(saveJson, i)
                print(json.dumps(rst, indent = 4))
            elif selection == "b" and not addr.isRoot:
                addr = addrSave.pop()
            elif selection == "q":
                print("저장하지 않은 내용은 전부 날아갔습니다!\n감사하십시오.")
                exit(1)
            elif selection == "c":
                os.system('cls')
            elif selection == "i" and addr.isRoot:
                root.updateNewVariable()
                print(saveJson)
        else:
            print(f"설정할 값을 입력해주세요.\n{addr.text} : {addr.text} -> ?\n* 설정을 원치 않으시다면 숫자가 아닌 것을 입력해주세요. *")
            value = input('>>> ')
            if value.isdigit():
                int(value)
            else:
                addr = addrSave.pop()