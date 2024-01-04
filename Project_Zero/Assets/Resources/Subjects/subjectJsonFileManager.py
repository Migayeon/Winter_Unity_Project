import csv, json
def convertCsvToJson():
    f = open("./metaSubjectInfo.csv", "r", encoding='UTF8')
    rd = csv.reader(f)
    NAMES = {
        "theory" : 0,
        "mana" : 1,
        "craft" : 2,
        "element" : 3,
        "attack" : 4
    }
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
        temp["id"] = id
        temp["subjectGroupId"] = 0
        temp["root"] = 0
        temp["needCount"] = needCount

        jsonContents = json.dumps(temp, ensure_ascii = False, indent=4)
        jsonFile = open(f"./{id}.json", 'w', encoding='UTF8')
        jsonFile.write(jsonContents)
        jsonFile.close()
    f.close()