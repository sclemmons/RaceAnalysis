
select * from requestcontexts where raceid='imbrazil2017' order by AgeGroupId


delete Triathletes 
where RequestContextId in
(select RequestContextId from RequestContexts 
where raceid='imbrazil2017')

delete RequestContexts where raceId = 'imbrazil2017' 