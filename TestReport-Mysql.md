## Performance Test Report

Provider: Mysql

Test time: 2022/07/28 17:13:40

Customer: 1,000,000

Product: 100,000

Order: 1,000,000

Bellow is test performance report. Lower is better

|Test case|Constraint|Non Constraint|
|--|--|--|
|Search Product|9ms|18ms|
|Search Product|10ms|10ms|
|Search Product|10ms|10ms|
|Search Product|11ms|9ms|
|Search Product|12ms|11ms|
|Avg|10.4ms|11.6ms|
|Get revenue last month|18339ms|30040ms|
|Get revenue last month|17537ms|29008ms|
|Get revenue last month|18727ms|29313ms|
|Get revenue last month|23186ms|28419ms|
|Get revenue last month|18451ms|28217ms|
|Avg|19248ms|28999.4ms|
|Get revenue this year|42814ms|41707ms|
|Get revenue this year|44519ms|44101ms|
|Get revenue this year|41859ms|42521ms|
|Get revenue this year|44087ms|43225ms|
|Get revenue this year|42555ms|43110ms|
|Avg|43166.8ms|42932.8ms|
|Best seller product this year|54465ms|58281ms|
|Best seller product this year|53301ms|59185ms|
|Best seller product this year|54911ms|53613ms|
|Best seller product this year|52125ms|54877ms|
|Best seller product this year|51040ms|53778ms|
|Avg|53168.4ms|55946.8ms|
|Top customer this year|57780ms|56449ms|
|Top customer this year|57745ms|58399ms|
|Top customer this year|55859ms|60480ms|
|Top customer this year|55017ms|59241ms|
|Top customer this year|55879ms|56786ms|
|Avg|56456ms|58271ms|
|Insert customer|20ms|29ms|
|Insert customer|17ms|20ms|
|Insert customer|18ms|23ms|
|Insert customer|19ms|23ms|
|Insert customer|20ms|18ms|
|Avg|18.8ms|22.6ms|
|Update customer|19ms|19ms|
|Update customer|26ms|25ms|
|Update customer|19ms|23ms|
|Update customer|19ms|19ms|
|Update customer|17ms|18ms|
|Avg|20ms|20.8ms|
|Delete customer|31ms|25ms|
|Delete customer|28ms|22ms|
|Delete customer|27ms|26ms|
|Delete customer|31ms|27ms|
|Delete customer|31ms|31ms|
|Avg|29.6ms|26.2ms|
