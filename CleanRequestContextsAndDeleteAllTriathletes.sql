

delete Triathletes 
where RequestContextId in
(select RequestContextId from RequestContexts 
where raceid='imswitz2016')

delete RequestContexts where raceId = 'imswitz2016'