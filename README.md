# Physical activity pattern complexity analysis

This project came up from [my PhD research](<doc/OM PhD thesis.pdf>) at Glasgow Caledonian University. It contains source code for analysis of objectively measured physical activity.

[![CircleCI](https://circleci.com/gh/o-mdr/pa-pattern-complexity/tree/main.svg?style=svg)](https://circleci.com/gh/o-mdr/pa-pattern-complexity/tree/main)

## Results

This repository mostly contains the _source code_. _Data_ is available in [a separate data repository](https://github.com/o-mdr/nhanes-pam-csv-data). This because the size of data is quite large and it's easier to separate intermediate data processing from the released data sets.

## Getting Started

Tested on Ubuntu 20.04 LTS system, you may need to adjust scripts for other platforms. C++ code also works on Windows platforms (x86 and x64), but scripting is only available on Ubuntu (or similar Linux systems).

```bash
git clone https://github.com/o-mdr/pa-pattern-complexity.git
cd pa-pattern-complexity
sudo ./script/bootstrap
./script/download-data
```

### Prerequisites

`./script/bootstrap` contains all the required software. To minimise friction consider using Ubuntu system for analysis.

### Data sources

The following data sources are used in this project:

- [NHANES 2003 - 2004 PAM](https://wwwn.cdc.gov/nchs/nhanes/search/datapage.aspx?Component=Examination&CycleBeginYear=2003)
- [NHANES 2005 - 2006 PAM](https://wwwn.cdc.gov/nchs/nhanes/search/datapage.aspx?Component=Examination&CycleBeginYear=2005)
- [NHANES 2011 - 2012 PAM](https://wwwn.cdc.gov/nchs/nhanes/search/datapage.aspx?Component=Examination&CycleBeginYear=2011)
- [NHANES 2013 - 2014 PAM](https://wwwn.cdc.gov/nchs/nhanes/search/datapage.aspx?Component=Examination&CycleBeginYear=2013)

### Running the build

```bash
# Using meson build system
meson setup builddir
meson compile -C builddir

# Or you could use Visual Studio, open solution file in
# src\nhanes.vs.sln
```

## Running data cleanup

Described in [NHANES Data cleanup](doc/NHANES-Data-cleanup.md).

## Built With

- [R](https://cran.r-project.org/) - Statistical computing and graphics environment
- [CircleCI](https://app.circleci.com/pipelines/github/o-mdr/pa-pattern-complexity) - CI server
- [Meson](https://mesonbuild.com) - modern C++ build tool
- [Visual Studio Code](https://code.visualstudio.com/) - IDE
- [Visual Studio](https://visualstudio.microsoft.com/) - IDE

## Contributing

Any contributions are welcome, please read [contributing guide](.github/CONTRIBUTING.md) for the process for submitting pull requests. Code of Conduct: [contributor covenant](.github/CODE_OF_CONDUCT.md).

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/your/project/tags).

## Acknowledgments

- https://wwwn.cdc.gov/nchs/nhanes for providing valuable data

## FAQ

**Q**: Download script crashes, I think this is because my machine ran out of memory.

**A**: Conversion from XPT to CSV is done inefficiently. A workaround for this is to increase a swap file. On my machine 16 GB RAM + 16 GB swap file seem to work OK.

## Authors

- **Oleksii Mandrychenko** - *Initial work*

## License

[Apache License, Version 2.0](LICENSE)
