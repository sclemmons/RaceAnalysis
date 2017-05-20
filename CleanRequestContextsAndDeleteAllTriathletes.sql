
select * from requestcontexts where raceid='implacid2014' order by AgeGroupId


delete Triathletes 
where RequestContextId in
(select RequestContextId from RequestContexts 
where raceid='implacid2014')

delete RequestContexts where raceId = 'implacid2014' 