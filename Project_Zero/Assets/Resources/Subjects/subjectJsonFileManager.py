import csv, json
def convertCsvToJson(fileName, infoFileName = "subjectsInfo"):
    fileName = fileName if ".csv" in fileName else fileName + ".csv"
    f = open(f"./{fileName}", "r", encoding='UTF8')
    rd = csv.reader(f)
    NAMES = {
        "theory" : 0,
        "mana" : 1,
        "craft" : 2,
        "element" : 3,
        "attack" : 4
    }
    cnt = 0
    for line in rd:
        temp = {}
        id = int(line[0])
        tier = int(line[1])
        name = line[2]
        enforceContents = dict(zip(map(lambda x : NAMES[x], line[3].split("+")), map(int, line[4].split("+"))))
        if line[5] == '':
            nextSubjects = []
        else:
            nextSubjects = list(map(int, line[5].split(',')))
        needCount = int(line[6])

        temp["id"] = id
        temp["tier"] = tier
        temp["name"] = name
        temp["enforceContents"] = enforceContents
        temp["nextSubjects"] = nextSubjects
        temp["subjectGroupId"] = 0
        temp["root"] = 0
        temp["needCount"] = needCount

        jsonContents = json.dumps(temp, ensure_ascii = False, indent=4)
        jsonFile = open(f"./{id}.json", 'w', encoding='UTF8')
        jsonFile.write(jsonContents)
        jsonFile.close()
        cnt += 1
    temp = {}
    temp["count"] = cnt
    temp["enforceTypeName"] = [
        "마법 이론",
        "마나 감응",
        "손재주",
        "속성력",
        "영창"
    ]
    temp["groupCount"] = 3
    jsonFile = open(f"./{infoFileName}.json", 'w', encoding='UTF8')
    jsonFile.write(json.dumps(temp, ensure_ascii = False, indent=4))
    jsonFile.close()
    f.close()

def convertJsonToCsv(fileName, infoFileName = "subjectsInfo"):
    NAMES = {
        "theory" : 0,
        "mana" : 1,
        "craft" : 2,
        "element" : 3,
        "attack" : 4
    }
    fileName = fileName if ".csv" in fileName else fileName + ".csv"
    jsonFile = open(f"./{infoFileName}.json", 'r', encoding='UTF8')
    info = json.load(jsonFile)
    jsonFile.close()
    csvList = []
    for i in range(info["count"]):
        jsonFile = open(f"./{i}.json", 'r', encoding='UTF8')
        jsonContents = dict(json.loads(jsonFile.read()))
        jsonFile.close()
        if i == 0:
            csvList.append(list(jsonContents.keys()))
        csvList.append(list(map(str, jsonContents.values())))
    csvFile = open(f"./{fileName}", 'w', encoding='UTF8')
    writer = csv.writer(csvFile)
    writer.writerows(csvList)
    csvFile.close()

def flattenDictionary(infoFileName = "subjectsInfo"):
    NAMES = {
        "theory" : 0,
        "mana" : 1,
        "craft" : 2,
        "element" : 3,
        "attack" : 4
    }
    jsonFile = open(f"./{infoFileName}.json", 'r', encoding='UTF8')
    info = json.load(jsonFile)
    jsonFile.close()
    for i in range(info["count"]):
        jsonFile = open(f"./{i}.json", 'r', encoding='UTF8')
        jsonContents = dict(json.loads(jsonFile.read()))
        jsonFile.close()
        print("?", jsonContents)
        a = jsonContents["enforceContents"]
        b = [0, 0, 0, 0, 0]
        print("?", a)
        for j in a.keys():
            b[int(j)] = a[j]
        jsonContents["enforceContents"] = b
        jsonWrite = open(f"./{i}.json", 'w', encoding='UTF8')
        #print(json.dumps(jsonContents, ensure_ascii = False, indent=4))
        jsonWrite.write(json.dumps(jsonContents, ensure_ascii = False, indent=4))
        jsonWrite.close()
convertCsvToJson("metaSubjectInfo")
#convertJsonToCsv("test")
flattenDictionary()
