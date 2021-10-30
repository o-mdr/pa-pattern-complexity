# Processing of NHANES data

## Counting number of files at some root of the data folder
```bash
# from the root of the project
# cd ~/git/pa-pattern-complexity
DATA_DIR=data
STUDY_C_COUNT=$(find $DATA_DIR/study-c -type f | wc -l)
STUDY_D_COUNT=$(find $DATA_DIR/study-d -type f | wc -l)
STUDY_G_COUNT=$(find $DATA_DIR/study-g -type f | wc -l)
STUDY_H_COUNT=$(find $DATA_DIR/study-h -type f | wc -l)
printf "Count of files: \n"`
  `"Study C ($DATA_DIR/study-c): $STUDY_C_COUNT \n"`
  `"Study D ($DATA_DIR/study-d): $STUDY_D_COUNT \n"`
  `"Study G ($DATA_DIR/study-g): $STUDY_G_COUNT \n"`
  `"Study H ($DATA_DIR/study-h): $STUDY_H_COUNT \n" | tee $DATA_DIR/file_count.txt
```

## Splitting large CSV files into smaller ones

```bash
./builddir/nhanes-csv-split -f "data/paxraw_c.xpt.csv" -o "data/study-c"
./builddir/nhanes-csv-split -f "data/paxraw_d.xpt.csv" -o "data/study-d"
./builddir/nhanes-csv-split -f "data/PAXMIN_G.XPT.csv" -o "data/study-g"
./builddir/nhanes-csv-split -f "data/PAXMIN_H.XPT.csv" -o "data/study-h"
```
