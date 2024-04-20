# ImTexterApi

This Repo is built using .net Core 8
IDE : Visual Studio 2022 (Community Edition)

To run this application 
You need to open it in visual studio and hit on run - Swagger supported UI will open Up.

![image](https://github.com/aravindapk/ImTexterApiNetCore/assets/48381916/4ea9bdcc-a516-4170-8226-56e26540d8d2)

Extract Imaeges API 
Type: Get
API will fetch the images by loading the Html using HtmlAgility pack and logic uses the img and background images filtered from html document.
You enter the url in the input box of swagger and hit on execute.
![image](https://github.com/aravindapk/ImTexterApiNetCore/assets/48381916/c2a9a708-b0f2-48cd-ab51-aa610beccea0)

Clear cache API.
Type: Get
This is used to clear the memory cache for the object which are cached during the fetch event. user has to supply the cacheKey in the format of ImageExtractor_<url> and TextAnalyzer_<url> for Image Extractor and Text Analyzer APIs respectively.

![image](https://github.com/aravindapk/ImTexterApiNetCore/assets/48381916/d50bb270-59b7-46ad-9279-a080c56b77e4)


Text Analyzer API
Type: Post
Used to analyze the words in the html document, this API will extract the words and ranks the top 10 words in whole document using the iterative steps. This also uses the HtmlAgility pack to frame the document.
In the swagger app you just enter the URL in the model format , for extensive analysis you can skip the words which are not necessary to be included in the ranking of words.
![image](https://github.com/aravindapk/ImTexterApiNetCore/assets/48381916/4f1bb2d5-dbfa-44b0-8006-56a306b15be2)

![image](https://github.com/aravindapk/ImTexterApiNetCore/assets/48381916/72739538-062b-4b66-a66a-05d759bbc267)

enter the words to exclude in the request body and execute, you will see these words will be skipped in the ranks
![image](https://github.com/aravindapk/ImTexterApiNetCore/assets/48381916/ecd0589a-4b85-4901-80ee-e2a51f4913c6)

![image](https://github.com/aravindapk/ImTexterApiNetCore/assets/48381916/8f17b060-0768-440f-909d-d6d0aea79281)



