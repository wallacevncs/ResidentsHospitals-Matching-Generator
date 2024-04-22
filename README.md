## ResidentsHospitals-Matching-Generator

This project is designed to generate a dataset for use in the Resident Matching Problem, inspired by the National Resident Matching Program (NRMP).


## Overview

The solution consists of two main projects: "ResidentDataset" and "PreferenceLists."

**ResidentDataset**

The "ResidentDataset" project takes as input a CSV file containing a list of names and randomly selects names from this list to represent the residents, up to the specified limit of total applicants. The idea is to have a significantly larger list of names than the total number of applicants to enable the creation of diverse and distinct datasets.

**PreferenceLists**

The "PreferenceLists" project processes files containing residents/candidates for residency programs. It also creates hospitals with their respective capacities, constrained by the total positions available. Using this information, random preference lists in JSON format are generated for both residents and hospitals.


## Usage
This tool can be useful for researchers, educators, and data enthusiasts interested in simulating and studying the allocation of residents to hospitals, inspired by the NRMP-style matching system.
## License
This project is under the license [MIT](./LICENSE.txt).